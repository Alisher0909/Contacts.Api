using ContactsApi.Entities;

namespace ContactsApi.Repositories.Abstractions;

public interface IContactRepository
{
    ValueTask<Entities.Contact> InsertAsync(Entities.Contact contact, CancellationToken cancellationToken = default);
    ValueTask<IEnumerable<Entities.Contact>> GetAllAsync(CancellationToken cancellationToken = default);
    ValueTask<Entities.Contact> GetSingleAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<Entities.Contact> UpdateAsync(Entities.Contact contact, CancellationToken cancellationToken = default);
    ValueTask<Entities.Contact> UpdateSinglePartAsync(Entities.Contact contact, CancellationToken cancellationToken = default);
    ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<bool> IsPhoneExistsAsync(string PhoneNumber, CancellationToken cancellationToken = default);
    ValueTask<bool> IsEmailExistsAsync(string Email, CancellationToken cancellationToken = default);
    ValueTask<bool> IsIdExistsAsync(int id, CancellationToken cancellationToken = default);
}