namespace ContactsApi.Models;

public record PatchContact(
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumber,
    string? Address
);