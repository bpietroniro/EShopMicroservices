namespace BuildingBlocks.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message) { }

        public BadRequestException(string message, string deatils)
            : base(message)
        {
            Details = Details;
        }

        public string? Details { get; }
    }
}
