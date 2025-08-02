using ContactsApi.Dtos;
using ContactsApi.Models;
using ContactsApi.Exceptions;

namespace ContactsApi.Services;

public class ContactService : IContactService
{
    private readonly List<Contact> _contacts = [];

    public Task<IEnumerable<ContactDto>> GetAllAsync(int page, int limit, string? query)
    {
        page = page <= 0 ? 1 : page;
        limit = limit <= 0 ? 10 : limit;

        var filtered = _contacts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            query = query.ToLower();
            filtered = filtered.Where(c =>
                c.FirstName.ToLower().Contains(query) ||
                c.LastName.ToLower().Contains(query) ||
                c.Email.ToLower().Contains(query));
        }

        var result = filtered
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(c => ToDto(c));

        return Task.FromResult<IEnumerable<ContactDto>>(result.ToList());
    }

    public Task<ContactDto?> GetByIdAsync(int id)
    {
        var contact = _contacts.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(contact is null ? null : ToDto(contact));
    }

    public async Task<ContactDto> CreateAsync(CreateContactDto dto)
    {
        if (await EmailExistsAsync(dto.Email))
            throw new CustomConflictException("Email is already in use.");

        if (await PhoneExistsAsync(dto.PhoneNumber))
            throw new CustomConflictException("Phone number is already in use.");

        var contact = new Contact
        {
            Id = _contacts.Count == 0 ? 1 : _contacts.Max(c => c.Id) + 1,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _contacts.Add(contact);
        return ToDto(contact);
    }

    public async Task UpdateAsync(int id, UpdateContactDto dto)
    {
        var contact = _contacts.FirstOrDefault(c => c.Id == id);
        if (contact is null)
            throw new CustomNotFoundException("Contact not found.");

        if (await EmailExistsAsync(dto.Email, id))
            throw new CustomConflictException("Email is already in use.");

        if (await PhoneExistsAsync(dto.PhoneNumber, id))
            throw new CustomConflictException("Phone number is already in use.");

        contact.FirstName = dto.FirstName;
        contact.LastName = dto.LastName;
        contact.Email = dto.Email;
        contact.PhoneNumber = dto.PhoneNumber;
        contact.Address = dto.Address;
        contact.UpdatedAt = DateTime.UtcNow;
    }

    public async Task PatchAsync(int id, PatchContactDto dto)
    {
        var contact = _contacts.FirstOrDefault(c => c.Id == id);
        if (contact is null)
            throw new CustomNotFoundException("Contact not found.");

        if (dto.Email is not null && await EmailExistsAsync(dto.Email, id))
            throw new CustomConflictException("Email is already in use.");

        if (dto.PhoneNumber is not null && await PhoneExistsAsync(dto.PhoneNumber, id))
            throw new CustomConflictException("Phone number is already in use.");

        if (dto.FirstName is not null) contact.FirstName = dto.FirstName;
        if (dto.LastName is not null) contact.LastName = dto.LastName;
        if (dto.Email is not null) contact.Email = dto.Email;
        if (dto.PhoneNumber is not null) contact.PhoneNumber = dto.PhoneNumber;
        if (dto.Address is not null) contact.Address = dto.Address;

        contact.UpdatedAt = DateTime.UtcNow;
    }

    public Task DeleteAsync(int id)
    {
        var contact = _contacts.FirstOrDefault(c => c.Id == id);
        if (contact is null)
            throw new CustomNotFoundException("Contact not found.");

        _contacts.Remove(contact);
        return Task.CompletedTask;
    }

    public Task<bool> EmailExistsAsync(string email, int? excludeId = null)
    {
        return Task.FromResult(
            _contacts.Any(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                            && (!excludeId.HasValue || c.Id != excludeId.Value))
        );
    }

    public Task<bool> PhoneExistsAsync(string phoneNumber, int? excludeId = null)
    {
        return Task.FromResult(
            _contacts.Any(c => c.PhoneNumber == phoneNumber
                            && (!excludeId.HasValue || c.Id != excludeId.Value))
        );
    }

    private static ContactDto ToDto(Contact contact)
    {
        return new ContactDto
        {
            Id = contact.Id,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Email = contact.Email,
            PhoneNumber = contact.PhoneNumber,
            Address = contact.Address,
            CreatedAt = contact.CreatedAt,
            UpdatedAt = contact.UpdatedAt
        };
    }
}