using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    [Serializable]
    public class InvalidColorProfileException : Exception
    {
        public InvalidColorProfileException() { }
        public InvalidColorProfileException(string message) : base(message) { }
        public InvalidColorProfileException(string message, Exception inner) : base(message, inner) { }
        protected InvalidColorProfileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
