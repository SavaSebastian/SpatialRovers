using Microsoft.AspNetCore.Mvc;
using SpatialRovers.Core.Dtos;
using SpatialRovers.Core.Entities;

namespace SpatialRovers.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlateauController : ControllerBase
{
    private Plateau _plateau;
    public PlateauController(Plateau plateau)
    {
        _plateau = plateau;
    }

    [HttpPost]
    [Route("initializerovers")]
    public IActionResult InitializeRovers(string[][] file)
    {
        var old = _plateau.VisitedPositions.Aggregate("", (current, position) => $"{current} {position.X}:{position.Y}").Trim();
        Console.WriteLine($"Old Plateau: {old}"); // Showcase DI
        // Assumes a new Plateau is needed when uploading a file as customizable Plateau settings are not implemented 
        _plateau.Rovers.Clear();

        try
        {
            foreach(var line in file)
            {
                _plateau.LaunchRover(line[0]);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(new
        {
            msg = "Rovers added successfully"
        });
    }

    [HttpGet]
    [Route("startall")]
    public IActionResult StartAllRovers()
    {
        var roversPositions = new List<List<Position>>();

        try
        {
            _plateau.Rovers.ForEach(r => {
                r.RunSequence();
                roversPositions.Add(r.VisitedPositions);
            });

        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
        return Ok(roversPositions);
    }
}