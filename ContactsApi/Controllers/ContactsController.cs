using AutoMapper;
using ContactsApi.Abstractions;
using ContactsApi.Dtos;
using ContactsApi.Models;
using ContactsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Controllers;

[ApiController, Route("api/[controller]")]
public class ContactsController(IContactService contactService, IMapper mapper) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactDto dto, CancellationToken abortionToken = default)
    {
        var contact = await contactService.CreateContactAsync(mapper.Map<CreateContact>(dto), abortionToken);
        return Ok(mapper.Map<ContactDto>(contact));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken abortionToken = default)
    {
        var result = await contactService.GetAllContactsAsync(abortionToken);
        return Ok(result.Select(mapper.Map<ContactDto>));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSingle(int id, CancellationToken abortionToken = default)
    {
        var singleContact = await contactService.GetSingleContactAsync(id, abortionToken);
        return Ok(mapper.Map<ContactDto>(singleContact));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, [FromBody] UpdateContactDto dto ,CancellationToken abortionToken = default)
    {
        var updated = await contactService.UpdateContactAsync(id, mapper.Map<UpdateContact>(dto) ,abortionToken);
        return Ok(mapper.Map<ContactDto>(updated));
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateSinglePropertyAsync(int id, [FromBody] PatchContactDto dto ,CancellationToken abortionToken = default)
    {
        var updated = await contactService.UpdateSinglePartOfContactAsync(id, mapper.Map<PatchContact>(dto) ,abortionToken);
        return Ok(mapper.Map<ContactDto>(updated));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContactAsync(int id, CancellationToken abortionToken = default)
    {
        await contactService.DeleteContactAsync(id ,abortionToken);
        return Ok($"Contact with id '{id}' is deleted");
    }
}