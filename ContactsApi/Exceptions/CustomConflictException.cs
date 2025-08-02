namespace ContactsApi.Exceptions;

public class CustomConflictException : Exception
{
    public CustomConflictException(string message) : base(message) { }
}