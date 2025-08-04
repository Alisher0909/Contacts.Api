namespace ContactsApi.Exceptions;

public class CustomBadRequestException(string errorMessage)
     : Exception(errorMessage) { }
