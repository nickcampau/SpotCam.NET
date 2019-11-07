using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotCam
{
    public enum EnvironmentalIssue
    {
        Unknown,

        /// <summary>
        /// The camera needs the luminance applied to the 
        /// imaging sensor to be more intense.
        /// </summary>
        MoreLuminanceNeeded,

        /// <summary>
        /// The camera needs the luminance applied to the 
        /// imaging sensor to be less intense.
        /// </summary>
        LessLuminanceNeeded,

        /// <summary>
        /// The luminance applied to the imaging sensor
        /// is fluctuating too much.
        /// </summary>
        UnstableLuminance,

        /// <summary>
        /// The luminance of the optical system must be zero.
        /// All light must be blocked to the imaging sensor.
        /// </summary>
        ZeroLuminanceRequired,

        /// <summary>
        /// The camera's filter slider control is not positioned
        /// in the Color Position.
        /// </summary>
        SliderNotInColorFilterPosition,

        /// <summary>
        /// The camera's filter slider control is not positioned
        /// in the Clear Position.
        /// </summary>
        SliderNotInClearPosition
    }

    [Serializable]
    public class EnvironmentalException : Exception
    {
        public EnvironmentalException()
            : base()
        {
            Issue = EnvironmentalIssue.Unknown;
        }

        public EnvironmentalException(EnvironmentalIssue issue)
            : base()
        {
            Issue = issue;
        }
        
        public EnvironmentalException(EnvironmentalIssue issue, string message)
            : base(message)
        {
            Issue = issue;
        }
        
        public EnvironmentalException(EnvironmentalIssue issue, string message, Exception inner)
            : base(message, inner)
        {
            Issue = issue;
        }

        protected EnvironmentalException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public EnvironmentalIssue Issue { get; private set; }

        public override string ToString()
        {
            return String.IsNullOrEmpty(Message) ? Issue.ToString() : base.ToString();
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            if (null == info)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("SpotCam.EnvironmentalIssue", this.Issue); 
            base.GetObjectData(info, context);
        }
    }
}
