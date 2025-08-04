using AutoMapper;
using ContactsApi.Models;
using ContactsApi.Dtos;
using ContactsApi.Exceptions;
using ContactsApi.Services.Abstractions;

namespace ContactsApi.Services;

public class ContactService(IMapper mapper) : IContactService
{
    private readonly Dictionary<string, Contact> contacts = [];
    private int idIndex = 1;

    public ValueTask<Contact> CreateContactAsync(CreateContact contact, CancellationToken cancellationToken)
    {
        if (contacts.ContainsKey(contact.PhoneNumber!))
            throw new CustomConflictException($"Contact with this '{contact.PhoneNumber}' already exists.");

        var newContact = mapper.Map<Contact>(contact);
        newContact.Id = idIndex++;
        newContact.CreatedAt = DateTimeOffset.Now;
        contacts.Add(newContact.PhoneNumber!, newContact);

        return ValueTask.FromResult(newContact);
    }

    public ValueTask<IEnumerable<Contact>> GetAllAsync(CancellationToken cancellationToken)
        => ValueTask.FromResult(contacts.Values.AsEnumerable());

    public ValueTask<Contact?> GetSingleOrDefaultAsync(int id, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(contacts.Values.FirstOrDefault(c => c.Id == id));

    public async ValueTask<Contact> GetSingleAsync(int id, CancellationToken cancellationToken = default)
        => await GetSingleOrDefaultAsync(id, cancellationToken)
            ?? throw new CustomNotFoundException($"Contact with id '{id}' not found!");

    public async ValueTask<Contact> UpdateContactAsync(int id, UpdateContact contact, CancellationToken cancellationToken = default)
    {
        var contactToUpdate = await GetSingleAsync(id, cancellationToken);
        var originalCreatedAt = contactToUpdate.CreatedAt;
        contacts.Remove(contactToUpdate.PhoneNumber!);
        mapper.Map(contact, contactToUpdate);
        contactToUpdate.CreatedAt = originalCreatedAt;
        contactToUpdate.UpdatedAt = DateTimeOffset.UtcNow;
        contacts[contactToUpdate.PhoneNumber!] = contactToUpdate;
        return contactToUpdate;
    }   

    public ValueTask<bool> ExistsAsync(string title, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(contacts.ContainsKey(title));

    public async ValueTask<Contact> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var contactToDelete = await GetSingleAsync(id, cancellationToken);
        contacts.Remove(contactToDelete.PhoneNumber!);

        return contactToDelete;
    }
    
    public async ValueTask<Contact> PatchAsync(int id, PatchContact patchContact, CancellationToken cancellationToken = default)
    {
        var contact = await GetSingleAsync(id, cancellationToken);

        if (!string.IsNullOrEmpty(patchContact.PhoneNumber) && patchContact.PhoneNumber != contact.PhoneNumber)
        {
            contacts.Remove(contact.PhoneNumber!);
            contact.PhoneNumber = patchContact.PhoneNumber;
            contacts[contact.PhoneNumber] = contact;
        }

        contact.FirstName = patchContact.FirstName ?? contact.FirstName;
        contact.LastName = patchContact.LastName ?? contact.LastName;
        contact.Email = patchContact.Email ?? contact.Email;
        contact.Address = patchContact.Address ?? contact.Address;
        contact.UpdatedAt = DateTimeOffset.Now;

        return contact;
    }
}