using AutoMapper;
using ContactsApi.Abstractions;
using ContactsApi.Models;
using ContactsApi.Repositories.Abstractions;

namespace ContactsApi.Services;

public class ContactService(IContactRepository repository, IMapper mapper) : IContactService
{
    public async ValueTask<Models.Contact> CreateContactAsync(CreateContact createdContact, CancellationToken cancellationToken = default)
    {
        var contact = await repository.InsertAsync(mapper.Map<Entities.Contact>(createdContact), cancellationToken);
        return mapper.Map<Models.Contact>(contact);
    }

    public async ValueTask<IEnumerable<Models.Contact>> GetAllContactsAsync(CancellationToken cancellationToken = default)
    {
        var contacts = await repository.GetAllAsync(cancellationToken);
        return contacts.Select(mapper.Map<Models.Contact>);
    }

    public async ValueTask<Models.Contact> GetSingleContactAsync(int id, CancellationToken cancellationToken = default)
    {
        var contact = await repository.GetSingleAsync(id, cancellationToken);
        return mapper.Map<Models.Contact>(contact);
    }

    public async ValueTask<Models.Contact> UpdateContactAsync(int id, UpdateContact contact, CancellationToken cancellationToken = default)
    {
        var found = await repository.GetSingleAsync(id, cancellationToken);
        found.UpdatedAt = DateTimeOffset.UtcNow;
        mapper.Map(contact,found);
        return mapper.Map<Models.Contact>(await repository.UpdateAsync(found, cancellationToken));
    }

    public async ValueTask<Models.Contact> UpdateSinglePartOfContactAsync(int id, PatchContact patchContact, CancellationToken cancellationToken = default)
    {
        var found = await repository.GetSingleAsync(id, cancellationToken);
        found.UpdatedAt = DateTimeOffset.UtcNow;
        mapper.Map(patchContact,found);
        return mapper.Map<Models.Contact>(await repository.UpdateSinglePartAsync(found, cancellationToken));
    }

    public async ValueTask DeleteContactAsync(int id, CancellationToken cancellationToken = default)
        => await repository.DeleteAsync(id, cancellationToken);

    public async ValueTask<bool> IsEmailExistsAsync(string Email, CancellationToken cancellationToken = default)
        => await repository.IsEmailExistsAsync(Email, cancellationToken);

    public async ValueTask<bool> IsPhoneExistsAsync(string PhoneNumber, CancellationToken cancellationToken = default)
        => await repository.IsPhoneExistsAsync(PhoneNumber, cancellationToken);

    public async ValueTask<bool> IsIdExistsAsync(int id, CancellationToken cancellationToken = default)
        => await repository.IsIdExistsAsync(id, cancellationToken);
}