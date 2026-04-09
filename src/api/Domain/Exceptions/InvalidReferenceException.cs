namespace Todo.Api.Domain.Exceptions;

/// <summary>
/// Thrown when <c>categoryId</c> or <c>urgencyLevelId</c> refers to a missing or inactive admin record.
/// Mapped to HTTP 422 by the global exception handler; no transport logic here.
/// </summary>
public sealed class InvalidReferenceException : Exception
{
    /// <summary>
    /// The request field that failed resolution: <c>categoryId</c> or <c>urgencyLevelId</c>.
    /// </summary>
    public string ReferenceField { get; }

    public InvalidReferenceException(string referenceField, string? message = null, Exception? innerException = null)
        : base(FormatMessage(referenceField, message), innerException)
    {
        ReferenceField = referenceField;
    }

    private static string FormatMessage(string referenceField, string? message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            return $"{referenceField}: {message}";
        }

        return $"Invalid or inactive reference for '{referenceField}'.";
    }
}
