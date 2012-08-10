using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    [Serializable]
    public class CameraNotSupportedException : CameraException
    {
        public CameraNotSupportedException() : base() { }
        public CameraNotSupportedException(string message) : base(message) { }
        public CameraNotSupportedException(string message, Exception inner) : base(message, inner) { }
        protected CameraNotSupportedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

}
