using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Claw.Validation
{
    /// <summary>
    /// Exception for strings containing invalid characters.
    /// </summary>
    [Serializable]
    public class IllegalCharacterException : Exception
    {
        public IllegalCharacterException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public IllegalCharacterException(string message)
            : base(message)
        { }

        public IllegalCharacterException() 
            : base()
        { }

        protected IllegalCharacterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
