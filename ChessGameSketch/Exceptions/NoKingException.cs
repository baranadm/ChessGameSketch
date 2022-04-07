using System.Runtime.Serialization;

namespace ChessGameSketch.Exceptions
{
    [Serializable]
    internal class NoKingException : ChessApiException
    {
        public NoKingException()
        {
        }

        public NoKingException(string? message) : base(message)
        {
        }

        public NoKingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoKingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}