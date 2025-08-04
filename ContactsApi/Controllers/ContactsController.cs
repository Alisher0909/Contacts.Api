using Microsoft.AspNetCore.Mvc;
using ContactsApi.Services;
using ContactsApi.Dtos;
using AutoMapper;
using ContactsApi.Models;
using ContactsApi.Services.Abstractions;
using Microsoft.AspNetCore.JsonPatch;

namespace ContactsApi.Controllers;

[ApiController, Route("api/[controller]")]
public class ContactsController(IContactService service, IMapper mapper) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactDto dto, CancellationToken cancellationToken = default
        )
    {
        var model = mapper.Map<CreateContact>(dto);
        var created = await service.CreateContactAsync(model, cancellationToken);
        return Ok(mapper.Map<ContactDto>(created));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var contacts = await service.GetAllAsync(cancellationToken);
        return Ok(mapper.Map<IEnumerable<ContactDto>>(contacts));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        var contact = await service.GetSingleAsync(id, cancellationToken);
        return Ok(mapper.Map<ContactDto>(contact));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateContactDto dto, CancellationToken cancellationToken = default)
    {
        var model = mapper.Map<UpdateContact>(dto);
        var updated = await service.UpdateContactAsync(id, model, cancellationToken);
        return Ok(mapper.Map<ContactDto>(updated));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var contact = await service.DeleteAsync(id, cancellationToken);
        return Ok(mapper.Map<ContactDto>(contact));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<PatchContactDto> patchDoc)
    {
        var existing = await service.GetSingleAsync(id);
        if (existing is null) return NotFound();

        var dto = mapper.Map<PatchContactDto>(existing);
        patchDoc.ApplyTo(dto);

        var updated = await service.PatchAsync(id, mapper.Map<PatchContact>(dto));
        return Ok(mapper.Map<ContactDto>(updated));
    }
}