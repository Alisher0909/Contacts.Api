namespace ContactsApi.Models;

public record UpdateContact(
    string FirstName,
    string LastName,
    string? Email,
    string? PhoneNumber,
    string? Address
);