namespace ContactsApi.Exceptions;

public class CustomConflictException(string errorMessage)
     : Exception(errorMessage) { }
