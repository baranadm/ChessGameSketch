using System.Runtime.Serialization;

namespace ChessGameApi.Exceptions
{
    [Serializable]
    public class FigureNotFoundException : ChessApiException
    {
        public FigureNotFoundException() : base("Figure not found.")
        {
        }

        public FigureNotFoundException(string? message) : base(message)
        {
        }

        public FigureNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected FigureNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}