using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    [Serializable]
    public class CameraBusyException : CameraException
    {
        public CameraBusyException() { }
        public CameraBusyException(string message) : base(message) { }
        public CameraBusyException(string message, Exception inner) : base(message, inner) { }
        protected CameraBusyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
