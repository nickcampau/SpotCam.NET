using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    [Serializable]
    public class FatalCameraException : CameraException
    {
        public FatalCameraException() { }
        public FatalCameraException(string message) : base(message) { }
        public FatalCameraException(string message, Exception inner) : base(message, inner) { }
        protected FatalCameraException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
