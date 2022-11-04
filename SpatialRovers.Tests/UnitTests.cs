using FluentAssertions;
using SpatialRovers.Core;
using SpatialRovers.Core.Dtos;
using SpatialRovers.Core.Entities;
using SpatialRovers.Core.Enums;
using SpatialRovers.Core.Extensions;
using System.Collections;
using Xunit.Abstractions;

namespace SpatialRovers.Tests;

public class UnitTests
{
    private readonly ITestOutputHelper _output;

    public UnitTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void TestEnumerableExtensions()
    {
        var direction = Direction.N;

        direction.Next().Should().Be(Direction.E);
        direction.Previous().Should().Be(Direction.W);
        direction.Next().Previous().Should().Be(Direction.N);
    }

    [Fact]
    public void TestEntities()
    {
        var plateau = new Plateau();
        plateau.LaunchRover("1 2 N|LMLMLMLMML");
        var rover = plateau.Rovers[0];

        rover.Position.Should().Be(new Position(1, 2));
        rover.Direction.Should().Be(Direction.N);

        var instructions = rover.Instructions;
        instructions.Should().HaveCount(10);

        rover.RunSequence();
        rover.Position.Should().Be(new Position(1, 3));
        rover.Direction.Should().Be(Direction.W);
    }

    [Theory]
    [ClassData(typeof(InstructionTestData))]
    public void RoverInstructionTest(
        List<Instruction> instructions,
        Position expectedPositon, 
        Direction expectedDirection, 
        bool shouldThrow, 
        bool isSequence = false)
    {
        var instructionQueue = new Queue<Instruction>();
        instructions.ForEach(i => instructionQueue.Enqueue(i));
        var rover = new Rover(new Position(0, 0), Direction.N, instructionQueue, new Plateau());

        try
        {
            if(isSequence) rover.RunSequence();
            else rover.Act();
        }
        catch(Exception ex)
        {
            if(shouldThrow)
            {
                ex.Should().NotBeNull();
                _output.WriteLine(ex.Message);
                return;
            }

            Assert.False(true);
        }

        rover.Position.Should().Be(expectedPositon);
        rover.Direction.Should().Be(expectedDirection);
    }
}

internal class InstructionTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new  object[] {
            new List<Instruction>() { Instruction.L },
            new Position(0, 0),
            Direction.W,
            false };

        yield return new  object[] { 
            new List<Instruction>() { Instruction.R },
            new Position(0, 0),
            Direction.E,
            false };

        yield return new  object[] { 
            new List<Instruction>() { Instruction.M },
            new Position(0, 1),
            Direction.N,
            false };

        yield return new  object[] {
            new List<Instruction>() { Instruction.L, Instruction.M },
            new Position(-1, 0),
            Direction.W,
            true,
            true };

        yield return new  object[] { 
            new List<Instruction>() { Instruction.R, Instruction.M },
            new Position(1, 0),
            Direction.E,
            false,
            true };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
