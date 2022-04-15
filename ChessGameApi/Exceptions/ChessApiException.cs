using System.Runtime.Serialization;

namespace ChessGameApi.Exceptions
{
    public class ChessApiException : Exception
    {
        public ChessApiException()
        {
        }

        public ChessApiException(string? message) : base(message)
        {
        }

        public ChessApiException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ChessApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
