using ContactsApi.Dtos;
using ContactsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController(IContactService contactService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int limit = 10, [FromQuery] string? query = null)
    {
        var contacts = await contactService.GetAllAsync(page, limit, query);
        return Ok(contacts);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var contact = await contactService.GetByIdAsync(id);
        if (contact is null)
            return NotFound();

        return Ok(contact);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactDto dto)
    {
        var created = await contactService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateContactDto dto)
    {
        await contactService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] PatchContactDto dto)
    {
        await contactService.PatchAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await contactService.DeleteAsync(id);
        return NoContent();
    }
}