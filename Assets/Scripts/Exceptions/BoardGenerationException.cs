using System;

namespace Services
{
    public class BoardGenerationException : Exception
    {
        public BoardGenerationException(string message) : base(message) { }
    }
}