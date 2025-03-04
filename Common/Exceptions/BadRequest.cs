namespace Common.Exceptions;

public class BadRequest(string? message) : Exception(message);