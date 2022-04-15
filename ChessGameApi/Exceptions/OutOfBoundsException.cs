using System.Runtime.Serialization;

namespace ChessGameApi.Exceptions
{
    public class OutOfBoundsException : ChessApiException
    {
        public OutOfBoundsException() : base("Given position is ot of bounds.")
        {
        }

        public OutOfBoundsException(string? message) : base(message)
        {
        }

        public OutOfBoundsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OutOfBoundsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}