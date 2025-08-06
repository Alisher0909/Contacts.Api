namespace ContactsApi.Models;

public record Contact(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? Address,
    DateTimeOffset? CreatedAt,
    DateTimeOffset? UpdatedAt)

{
    public int Id { get; set; }
}