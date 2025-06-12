namespace Products.Utility.Exceptions
{
    public class RepositoryException : Exception
    {
        public int StatusCode { get; }

        public RepositoryException(string message, int statusCode = 500)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}