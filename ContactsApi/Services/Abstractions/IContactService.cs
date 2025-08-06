using ContactsApi.Dtos;
using ContactsApi.Models;

namespace ContactsApi.Abstractions;

public interface IContactService
{
    ValueTask<Models.Contact> CreateContactAsync(CreateContact dto, CancellationToken cancellationToken = default);
    ValueTask<IEnumerable<Models.Contact>> GetAllContactsAsync(CancellationToken cancellationToken = default);
    ValueTask<Models.Contact> GetSingleContactAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<Models.Contact> UpdateContactAsync(int id, UpdateContact contact, CancellationToken cancellationToken = default);
    ValueTask<Models.Contact> UpdateSinglePartOfContactAsync(int id, PatchContact patchContact, CancellationToken cancellationToken = default);
    ValueTask DeleteContactAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<bool> IsPhoneExistsAsync(string PhoneNumber, CancellationToken cancellationToken = default);
    ValueTask<bool> IsEmailExistsAsync(string Email, CancellationToken cancellationToken = default);
    ValueTask<bool> IsIdExistsAsync(int id, CancellationToken cancellationToken = default);
}