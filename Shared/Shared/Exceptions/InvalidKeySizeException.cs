using System;

namespace Shared.Exceptions
{
    public class InvalidKeySizeException : Exception
    {
        public InvalidKeySizeException()
            : base("Key size must be divisible by, and greater than 8.")
        {
        }
    }
}
