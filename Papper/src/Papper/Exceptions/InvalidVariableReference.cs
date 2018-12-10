using System;

namespace Papper
{
    public class InvalidVariableException : Exception
    {
        public string Varibale { get; private set; }
        public InvalidVariableException(string variable) : base($"Could not resolve Variable <{variable}>!")
        {
            Varibale = variable;
        }
    }
}
