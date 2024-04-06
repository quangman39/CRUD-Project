namespace Exceptions
{
    public class InvalidIdException : ArgumentException
    {
        public InvalidIdException() : base() { }
        public InvalidIdException(string? message) : base(message) { }
        public InvalidIdException(string? message, Exception? innerException) { }
    }
}
