using System;

namespace Shared.Exceptions
{
    public class InvalidSaltSizeException : Exception
    {
        public InvalidSaltSizeException()
            : base("Salt size must be divisible by, and greater than 8.")
        {
        }
    }
}
