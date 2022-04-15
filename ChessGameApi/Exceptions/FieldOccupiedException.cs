using System.Runtime.Serialization;

namespace ChessGameApi.Exceptions
{
    public class FieldOccupiedException : ChessApiException
    {
        public FieldOccupiedException() : base("Field is occupied by another figure.")
        {
        }

        public FieldOccupiedException(string? message) : base(message)
        {
        }

        public FieldOccupiedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected FieldOccupiedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}