using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Monopoly.Warehouse.Core.Abstractions.Repositories;
using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;
using Monopoly.Warehouse.WebHost.Extensions;
using Monopoly.Warehouse.WebHost.Models.Pallet;

namespace Monopoly.Warehouse.WebHost.Controllers;

[Route("api/pallets")]
[ApiController]
public class PalletsController(IRepository<Pallet> palletsRepository, IValidator<PalletCreateOrUpdate> validator) : ControllerBase
{
    /// <summary>
    ///     Retrieves all pallets.
    /// </summary>
    /// <returns>A list of pallets.</returns>
    /// <response code="200">Returns the list of pallets</response>
    /// <response code="404">If no pallets are found</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PalletShortResponse>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IEnumerable<PalletShortResponse>>> GetPalletsAsync()
    {
        var result = await palletsRepository.GetAllAsync();

        if (result.IsNullOrEmpty())
            return NotFound();

        var pallets = result.AsEnumerable().Select(p => new PalletShortResponse(p)).ToList();

        return Ok(pallets.OrderByDescending(p => p.BoxesInside).ToList());
    }

    /// <summary>
    ///     Retrieves a pallet by ID.
    /// </summary>
    /// <param name="id">The ID of the dto to retrieve.</param>
    /// <returns>The requested dto.</returns>
    /// <response code="200">Returns the requested dto</response>
    /// <response code="404">If the dto is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PalletShortResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<PalletShortResponse>> GetPalletByIdAsync(Guid id)
    {
        Pallet? pallet = await palletsRepository.GetByIdAsync(id);

        if (pallet == null) return NotFound();

        var palletModel = new PalletShortResponse(pallet);

        return Ok(palletModel);
    }

    /// <summary>
    ///     Creates a new pallet.
    /// </summary>
    /// <param name="model">The dto object to create.</param>
    /// <returns>The created dto.</returns>
    /// <response code="201">Returns the created dto</response>
    /// <response code="500">If the Pallets set is null</response>
    [HttpPost]
    [ProducesResponseType(typeof(PalletResponse), 201)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<PalletResponse>> PostPallet(PalletCreateOrUpdate model)
    {
        ValidationResult result = await validator.ValidateAsync(model);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState.ValidationState);
        }

        var pallet = new Pallet
        {
            Id     = Guid.NewGuid(),
            Boxes  = new List<Box>(),
            Depth  = model.Depth,
            Height = model.Height,
            Width  = model.Width
        };

        try
        {
            await palletsRepository.CreateAsync(pallet);
            return CreatedAtAction("GetPalletById", new { id = pallet.Id }, pallet);
        }
        catch (Exception ex)
        {
            return Problem(title: "Pallet creation failed", detail: ex.Message, statusCode: 500);
        }
    }

    /// <summary>
    ///     Deletes a pallet by ID.
    /// </summary>
    /// <param name="id">The ID of the dto to delete.</param>
    /// <returns>An IActionResult indicating the outcome of the operation.</returns>
    /// <response code="204">If the deletion was successful</response>
    /// <response code="404">If the dto is not found</response>
    /// <response code="500">If other error occurred</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeletePallet(Guid id)
    {
        try
        {
            Pallet? pallet = await palletsRepository.GetByIdAsync(id);
            if (pallet == null)
                return NotFound();

            await palletsRepository.DeleteAsync(pallet);

            return NoContent();
        }
        catch (Exception e)
        {
            return Problem(title: "Pallet deletion failed", detail: e.Message, statusCode: 500);
        }
    }
}