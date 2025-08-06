using ContactsApi.Database;
using ContactsApi.Exceptions;
using ContactsApi.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Npgsql;   

namespace ContactsApi.Repositories;

public class ContactRepository(AppDbContext context) : IContactRepository
{    
    public async ValueTask<Entities.Contact> InsertAsync(Entities.Contact contact, CancellationToken cancellationToken = default)
    {
        try
        {
            contact.CreatedAt = DateTimeOffset.UtcNow;
            var entry = context.Contacts.Add(contact);
            await context.SaveChangesAsync(cancellationToken);
            return entry.Entity;
        }
        catch (NpgsqlException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            throw new CustomConflictException(ex.Message);
        }
    }

    public async ValueTask<Entities.Contact> UpdateAsync(Entities.Contact contact, CancellationToken cancellationToken = default)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return contact;
        }
        catch (NpgsqlException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            throw new CustomConflictException(ex.Message);
        }
    }

    public async ValueTask<Entities.Contact> UpdateSinglePartAsync(Entities.Contact contact, CancellationToken cancellationToken = default)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return contact;
        }
        catch (NpgsqlException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            throw new CustomConflictException(ex.Message);
        }
    }

    public async ValueTask<Entities.Contact> GetSingleAsync(int id, CancellationToken cancellationToken = default)
        => await context.Contacts.FindAsync([id], cancellationToken) ?? throw new CustomNotFoundException($"Contact with id '{id}' not found");

    public async ValueTask<IEnumerable<Entities.Contact>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.Contacts.ToListAsync(cancellationToken);

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var effectedRows = await context.Contacts.Where(c => c.Id == id).ExecuteDeleteAsync(cancellationToken);
        if (effectedRows < 1)
            throw new CustomNotFoundException($"Contact with id '{id}' not found to delete");
    }

    public ValueTask<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(context.Contacts.Any(c => c.Email == email));

    public ValueTask<bool> IsPhoneExistsAsync(string PhoneNumber, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(context.Contacts.Any(c => c.PhoneNumber == PhoneNumber));

    public ValueTask<bool> IsIdExistsAsync(int id, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(context.Contacts.Any(c => c.Id == id));
}