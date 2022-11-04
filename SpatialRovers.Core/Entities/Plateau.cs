using SpatialRovers.Core.Dtos;
using SpatialRovers.Core.Enums;

namespace SpatialRovers.Core.Entities;

public class Plateau
{
    public List<Rover> Rovers { get; init; }
    public int Height { get; init; }
    public int Width { get; init; }

    public IEnumerable<Position> VisitedPositions => Rovers.SelectMany(r => r.VisitedPositions).Distinct();
        
    public Plateau(int height = 6, int width = 6)
    {
        Height = height;
        Width = width;
        Rovers = new();
    }

    public void LaunchRover(string data)
    {
        var (landingData, instructionData) = GetRoverData(data);
        var (position, direction) = GetLandingInfo(landingData);
        var instructions = GetInstructions(instructionData);

        Rovers.Add(new Rover(position, direction, instructions, this));
    }

    public void StartRover(Guid id) => Rovers.FirstOrDefault(r => r.Id == id)?.RunSequence();

    private static Queue<Instruction> GetInstructions(string data)
    {
        var instructions = new Queue<Instruction>();

        foreach (var c in data.ToCharArray())
        {
            if (!Enum.TryParse(typeof(Instruction), c.ToString(), out var instruction))
                throw new ArgumentException("Invalid Instruction", nameof(data));

            instructions.Enqueue((Instruction)instruction);
        }

        return instructions;
    }

    private static (Position, Direction) GetLandingInfo(string data)
    {
        var landingInfo = data.Split(" ");

        if (landingInfo.Length != 3 ||
            !int.TryParse(landingInfo[0], out var x) ||
            !int.TryParse(landingInfo[1], out var y) ||
            !Enum.TryParse(typeof(Direction), landingInfo[2], out var direction))
            throw new ArgumentException("Invalid Landing Info", nameof(data));

        return (new Position(x, y), (Direction)direction);
    }

    private static (string, string) GetRoverData(string data)
    {
        var parsed = data.Split("|");
        if (parsed.Length != 2) throw new ArgumentException("Invalid Rover Data", nameof(data));

        return (parsed[0], parsed[1]);
    }
}