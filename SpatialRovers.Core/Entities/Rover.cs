using SpatialRovers.Core.Dtos;
using SpatialRovers.Core.Entities;
using SpatialRovers.Core.Enums;
using SpatialRovers.Core.Extensions;

namespace SpatialRovers.Core;

public class Rover
{
    public Guid Id { get; init; }
    public List<Position> VisitedPositions { get; init; }
    public Queue<Instruction> Instructions { get; init; }
    public Plateau Plateau { get; init; }
    public Position Position { get; set; }
    public Direction Direction { get; set; }

    public Rover(Position position, Direction direction, Queue<Instruction> instructions, Plateau plateau)
    {
        Id = new Guid();
        VisitedPositions = new List<Position>() { position };
        Instructions = instructions;
        Position = position;
        Direction = direction;
        Plateau = plateau;
    }

    public void RunSequence()
    {
        while (Instructions.Count > 0)
        {
            Act(Instructions.Dequeue());
        }
    }

    public Position Act(Instruction? instruction = null)
    {
        instruction ??= Instructions.Count > 0
            ? Instructions.Dequeue()
            : Instruction.StandBy;

        switch (instruction)
        {
            case Instruction.L:
                Direction = Direction.Previous();
                break;
            case Instruction.R:
                Direction = Direction.Next();
                break;
            case Instruction.M:
                Move();
                VisitedPositions.Add(Position);
                break;
            case Instruction.StandBy: break;
            default:
                throw new ArgumentOutOfRangeException(nameof(instruction), instruction, null);
        }

        return Position;
    }

    public void Move()
    {
        var (x, y) = Direction switch
        {
            Direction.N => (Position.X, Position.Y + 1),
            Direction.E => (Position.X + 1, Position.Y),
            Direction.S => (Position.X, Position.Y - 1),
            Direction.W => (Position.X - 1, Position.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(Direction), Direction, null)
        };

        if (x == Plateau.Width || x < 0 ||
            y == Plateau.Height || y < 0)
            throw new Exception("Rover tried to move out of plateau bounds!");

        Position = new Position(x, y);
    }
}