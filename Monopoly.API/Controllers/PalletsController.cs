using Microsoft.AspNetCore.Mvc;
using Monopoly.API.Data.DTOs;
using Monopoly.API.Data.Entities;
using Monopoly.API.Services.Interfaces;

namespace Monopoly.API.Controllers;

[Route("api/pallets")]
[ApiController]
public class PalletsController(MainDbContext context, IDataService<Pallet> pService) : ControllerBase
{
    /// <summary>
    ///     Retrieves all pallets.
    /// </summary>
    /// <returns>A list of pallets.</returns>
    /// <response code="200">Returns the list of pallets</response>
    /// <response code="404">If no pallets are found</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PalletDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IEnumerable<PalletDto>>> GetPallets()
    {
        var result = await pService.GetAllAsync();

        if (result == null) return NotFound();

        var pallets = result.AsEnumerable().Select(p => new PalletDto(p)).ToList();

        return pallets;
    }

    /// <summary>
    ///     Retrieves a pallet by ID.
    /// </summary>
    /// <param name="id">The ID of the dto to retrieve.</param>
    /// <returns>The requested dto.</returns>
    /// <response code="200">Returns the requested dto</response>
    /// <response code="404">If the dto is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PalletDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<PalletDto>> GetPallet(int id)
    {
        if (context.Pallets == null) return NotFound();

        var result = await pService.GetByIdAsync(id);

        if (result == null) return NotFound();

        var pallet = new PalletDto(result);

        return pallet;
    }

    /// <summary>
    ///     Creates a new pallet.
    /// </summary>
    /// <param name="dto">The dto object to create.</param>
    /// <returns>The created dto.</returns>
    /// <response code="201">Returns the created dto</response>
    /// <response code="500">If the Pallets set is null</response>
    [HttpPost]
    [ProducesResponseType(typeof(PalletDto), 201)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<PalletDto>> PostPallet(PalletDto dto)
    {
        var result = await pService.Create(new Pallet(dto));

        if (result == null) return Problem("Entity set 'MainDbContext.Pallets' is null.");

        return CreatedAtAction("GetPallet", new { id = dto.Id }, dto);
    }

    /// <summary>
    ///     Deletes a pallet by ID.
    /// </summary>
    /// <param name="id">The ID of the dto to delete.</param>
    /// <returns>An IActionResult indicating the outcome of the operation.</returns>
    /// <response code="204">If the deletion was successful</response>
    /// <response code="404">If the dto is not found</response>
    [HttpDelete("delete/{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeletePallet(int id)
    {
        bool? result = await pService.DeleteByIdAsync(id);

        if (result == null)
            return NotFound();

        if (result == false)
            return Problem("A problem occurred while saving changes in database");

        return NoContent();
    }
}