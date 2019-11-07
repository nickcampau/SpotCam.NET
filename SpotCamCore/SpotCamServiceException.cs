using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    [Serializable]
    public class SpotCamServiceException : Exception
    {
        public SpotCamServiceException(Interop.SpotCamReturnCode code)
        {
            ErrorCode = code;
        }
        public SpotCamServiceException(Interop.SpotCamReturnCode code, string message) : base(message)
        {
            ErrorCode = code;
        }
        public SpotCamServiceException(Interop.SpotCamReturnCode code, string message, Exception inner) : base(message, inner)
        {
            ErrorCode = code;
        }
        protected SpotCamServiceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public Interop.SpotCamReturnCode ErrorCode { get; private set; }

        public override string ToString()
        {
            return String.IsNullOrEmpty(Message) ? ErrorCode.ToString() : base.ToString();
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            if (null == info)
                throw new ArgumentNullException("info");
            info.AddValue("SpotCam.ReturnCode", this.ErrorCode);
            base.GetObjectData(info, context);
        }

    }
}
