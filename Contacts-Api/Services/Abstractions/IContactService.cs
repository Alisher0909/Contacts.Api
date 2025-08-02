using ContactsApi.Dtos;

namespace ContactsApi.Services;

public interface IContactService
{
    Task<IEnumerable<ContactDto>> GetAllAsync(int page, int limit, string? query);
    Task<ContactDto?> GetByIdAsync(int id);
    Task<ContactDto> CreateAsync(CreateContactDto dto);
    Task UpdateAsync(int id, UpdateContactDto dto);
    Task PatchAsync(int id, PatchContactDto dto);
    Task DeleteAsync(int id);

    Task<bool> EmailExistsAsync(string email, int? excludeId = null);
    Task<bool> PhoneExistsAsync(string phoneNumber, int? excludeId = null);
}