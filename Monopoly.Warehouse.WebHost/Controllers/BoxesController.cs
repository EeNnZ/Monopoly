using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Monopoly.Warehouse.Core.Abstractions.Repositories;
using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;
using Monopoly.Warehouse.WebHost.Models.Box;
using Monopoly.Warehouse.WebHost.Models.Pallet;
using Swashbuckle.AspNetCore.Annotations;

namespace Monopoly.Warehouse.WebHost.Controllers;

/// <summary>
/// </summary>
/// <param name="boxesRepository"></param>
[ApiController]
[Route("api/[controller]")]
public class BoxesController(IRepository<Box> boxesRepository) : ControllerBase
{
    /// <summary>
    ///     Gets all boxes.
    /// </summary>
    /// <returns>A list of boxes.</returns>
    /// <response code="200">Returns the list of boxes</response>
    /// <response code="404">If no boxes are found</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BoxResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Retrieve all boxes", Description = "Fetches all the boxes from the database.")]
    public async Task<ActionResult<IEnumerable<BoxShortResponse>>> GetBoxesAsync()
    {
        var result = await boxesRepository.GetAllAsync();

        if (result.IsNullOrEmpty())
            return NotFound();

        var boxes = result.AsEnumerable()
                          .Select(b => new BoxShortResponse
                           {
                               DateCreated    = b.DateCreated,
                               Depth          = b.Depth,
                               ExpirationDate = b.ExpirationDate,
                               Height         = b.Height,
                               Id             = b.Id,
                               PalletId       = b.PalletId,
                               Volume         = b.Volume,
                               Weight         = b.Weight,
                               Width          = b.Width
                           })
                          .ToList();

        return Ok(boxes);
    }

    /// <summary>
    ///     Gets a box by ID.
    /// </summary>
    /// <param name="id">ID of the box to retrieve.</param>
    /// <returns>The requested box.</returns>
    /// <response code="200">Returns the requested box</response>
    /// <response code="404">If the box is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BoxResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Retrieve a box by ID", Description = "Fetches a box from the database using its ID.")]
    public async Task<ActionResult<BoxResponse>> GetBoxByIdAsync(Guid id)
    {
        Box? box = await boxesRepository.GetByIdAsync(id);

        if (box == null) return NotFound();

        var boxModel = new BoxResponse
        {
            Id          = box.Id,
            Width       = box.Width,
            Height      = box.Height,
            DateCreated = box.DateCreated,
            PalletId    = box.PalletId,
            Pallet = box.Pallet is null
                ? null
                : new PalletResponse
                {
                    Id             = (Guid)box.PalletId!,
                    Boxes          = box.Pallet.Boxes,
                    BoxesInside    = box.Pallet.Boxes.Count,
                    Depth          = box.Pallet.Depth,
                    ExpirationDate = box.Pallet.ExpirationDate?.ToShortDateString(),
                    Height         = box.Pallet.Height,
                    Volume         = box.Pallet.Volume,
                    Weight         = box.Pallet.Weight,
                    Width          = box.Pallet.Width
                },
            Depth          = box.Depth,
            Weight         = 0,
            Volume         = 0,
            ExpirationDate = null
        };

        return Ok(boxModel);
    }

    /// <summary>
    ///     Creates a new box.
    /// </summary>
    /// <param name="model">The box object to create.</param>
    /// <returns>The created box.</returns>
    /// <response code="201">Returns the created box</response>
    /// <response code="500">If there is a problem with the entity set</response>
    [HttpPost]
    [ProducesResponseType(typeof(BoxResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Create a new box", Description = "Adds a new box to the database.")]
    public async Task<ActionResult<BoxResponse>> PostBox(BoxCreateOrUpdate model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.ValidationState);

        var box = new Box()
        {
            Weight = model.Weight,
            Id = Guid.NewGuid(),
        };

        try
        {
            await boxesRepository.CreateAsync(box);

            return CreatedAtAction("GetBoxById", new { id = box.Id }, new BoxResponse(box));
        }
        catch (Exception ex)
        {
            return Problem(title: "Error creating box", detail: ex.Message, statusCode: 500);
        }
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
    public async Task<IActionResult> DeleteBox(Guid id)
    {
        try
        {
            Box? box = await boxesRepository.GetByIdAsync(id);
            if (box == null)
                return NotFound();

            await boxesRepository.DeleteAsync(box);

            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(title: "Error deleting box", detail: ex.Message, statusCode: 500);
        }
    }
}