using AutoMapper;
using ContactsApi.Database;
using ContactsApi.Models;
using ContactsApi.Dtos;
using ContactsApi.Exceptions;
using ContactsApi.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Services;

public class ContactService(AppDbContext dbContext, IMapper mapper) : IContactService
{
    public async ValueTask<Contact> CreateContactAsync(CreateContact contact, CancellationToken cancellationToken)
    {
        var exists = await dbContext.Contacts
            .AnyAsync(c => c.PhoneNumber == contact.PhoneNumber, cancellationToken);

        if (exists)
            throw new CustomConflictException($"Contact with this phone number '{contact.PhoneNumber}' already exists.");

        var newContact = mapper.Map<Contact>(contact);
        newContact.CreatedAt = DateTimeOffset.UtcNow;

        await dbContext.Contacts.AddAsync(newContact, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return newContact;
    }

    public async ValueTask<IEnumerable<Contact>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Contacts.ToListAsync(cancellationToken);
    }

    public async ValueTask<Contact?> GetSingleOrDefaultAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Contacts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async ValueTask<Contact> GetSingleAsync(int id, CancellationToken cancellationToken = default)
    {
        var contact = await dbContext.Contacts
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return contact ?? throw new CustomNotFoundException($"Contact with id '{id}' not found!");
    }

    public async ValueTask<Contact> UpdateContactAsync(int id, UpdateContact updatedDto, CancellationToken cancellationToken = default)
    {
        var contact = await GetSingleAsync(id, cancellationToken);

        var originalCreatedAt = contact.CreatedAt;
        mapper.Map(updatedDto, contact);
        contact.CreatedAt = originalCreatedAt;
        contact.UpdatedAt = DateTimeOffset.UtcNow;

        dbContext.Contacts.Update(contact);
        await dbContext.SaveChangesAsync(cancellationToken);

        return contact;
    }

    public async ValueTask<Contact> PatchAsync(int id, PatchContact patchContact, CancellationToken cancellationToken = default)
    {
        var contact = await GetSingleAsync(id, cancellationToken);

        if (!string.IsNullOrEmpty(patchContact.PhoneNumber) && patchContact.PhoneNumber != contact.PhoneNumber)
        {
            var exists = await dbContext.Contacts
                .AnyAsync(c => c.PhoneNumber == patchContact.PhoneNumber, cancellationToken);
            if (exists)
                throw new CustomConflictException($"Contact with this phone number '{patchContact.PhoneNumber}' already exists.");

            contact.PhoneNumber = patchContact.PhoneNumber;
        }

        contact.FirstName = patchContact.FirstName ?? contact.FirstName;
        contact.LastName = patchContact.LastName ?? contact.LastName;
        contact.Email = patchContact.Email ?? contact.Email;
        contact.Address = patchContact.Address ?? contact.Address;
        contact.UpdatedAt = DateTimeOffset.UtcNow;

        dbContext.Contacts.Update(contact);
        await dbContext.SaveChangesAsync(cancellationToken);

        return contact;
    }

    public async ValueTask<Contact> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var contact = await GetSingleAsync(id, cancellationToken);

        dbContext.Contacts.Remove(contact);
        await dbContext.SaveChangesAsync(cancellationToken);

        return contact;
    }

    public async ValueTask<bool> ExistsAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await dbContext.Contacts.AnyAsync(c => c.PhoneNumber == phoneNumber, cancellationToken);
    }
}