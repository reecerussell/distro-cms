using System;

namespace Shared.Exceptions
{
    public class InvalidIterationCountException : Exception
    {
        public InvalidIterationCountException()
            : base("Iteration count cannot be less than 1.")
        {
        }
    }
}
