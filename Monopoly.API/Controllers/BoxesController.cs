using Microsoft.AspNetCore.Mvc;
using Monopoly.API.Data.DTOs;
using Monopoly.API.Data.Entities;
using Monopoly.API.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Monopoly.API.Controllers;

/// <summary>
/// </summary>
/// <param name="context"></param>
[ApiController]
[Route("api/[controller]")]
public class BoxesController(MainDbContext context, IDataService<Box> bService) : ControllerBase
{
    /// <summary>
    ///     Gets all boxes.
    /// </summary>
    /// <returns>A list of boxes.</returns>
    /// <response code="200">Returns the list of boxes</response>
    /// <response code="404">If no boxes are found</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BoxDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Retrieve all boxes", Description = "Fetches all the boxes from the database.")]
    public async Task<ActionResult<IEnumerable<BoxDto>>> GetBoxes()
    {
        var result = await bService.GetAllAsync();

        if (result == null) return NotFound();

        var boxes = result.AsEnumerable().Select(b => new BoxDto(b)).ToList();

        return boxes;
    }

    /// <summary>
    ///     Gets a box by ID.
    /// </summary>
    /// <param name="id">ID of the box to retrieve.</param>
    /// <returns>The requested box.</returns>
    /// <response code="200">Returns the requested box</response>
    /// <response code="404">If the box is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BoxDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Retrieve a box by ID", Description = "Fetches a box from the database using its ID.")]
    public async Task<ActionResult<BoxDto>> GetBox(int id)
    {
        if (context.Boxes == null) return NotFound();

        var result = await bService.GetByIdAsync(id);

        if (result == null) return NotFound();

        var box = new BoxDto(result);

        return box;
    }

    /// <summary>
    ///     Creates a new box.
    /// </summary>
    /// <param name="dto">The box object to create.</param>
    /// <returns>The created box.</returns>
    /// <response code="201">Returns the created box</response>
    /// <response code="500">If there is a problem with the entity set</response>
    [HttpPost]
    [ProducesResponseType(typeof(BoxDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Create a new box", Description = "Adds a new box to the database.")]
    public async Task<ActionResult<BoxDto>> PostBox(BoxDto dto)
    {
        var result = await bService.Create(new Box(dto));

        if (result == null) return Problem("Entity set 'MainDbContext.Boxes' is null.");


        return CreatedAtAction("GetBox", new { id = result.Id }, result);
    }

    /// <summary>
    ///     Deletes a box by ID.
    /// </summary>
    /// <param name="id">ID of the box to delete.</param>
    /// <returns>An IActionResult indicating the result of the operation.</returns>
    /// <response code="204">Success</response>
    /// <response code="404">If the box is not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Delete a box by ID", Description = "Removes a box from the database using its ID.")]
    public async Task<IActionResult> DeleteBox(int id)
    {
        bool? result = await bService.DeleteByIdAsync(id);

        if (result == null) return NotFound();

        if (result == false)
            return Problem("A problem occurred while saving changes in database");


        return NoContent();
    }
}