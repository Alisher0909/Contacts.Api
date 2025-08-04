namespace ContactsApi.Exceptions;

public class CustomNotFoundException(string errorMessage)
     : Exception(errorMessage) { }
