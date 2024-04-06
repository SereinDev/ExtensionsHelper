using System;

namespace ExtensionsHelper.Models.Exceptions;

[Serializable]
public class CheckFailureExceptionException : Exception
{
    public CheckFailureExceptionException() { }

    public CheckFailureExceptionException(string message)
        : base(message) { }

    public CheckFailureExceptionException(string message, Exception inner)
        : base(message, inner) { }
}
