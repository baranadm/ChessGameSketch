using System.Runtime.Serialization;

namespace ChessGameApi.Exceptions
{
    public class MoveNotAllowedException : ChessApiException
    {
        public MoveNotAllowedException() : base("This move is not allowed.")
        {
        }

        public MoveNotAllowedException(string? message) : base(message)
        {
        }

        public MoveNotAllowedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected MoveNotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
