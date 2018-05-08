using System;
using System.Runtime.Serialization;

namespace Pear {

    [DataContract]
    public class Metric {

        [DataMember]
        public float value { get; set; }
        [DataMember]
        public float recordTime { get; set; }

        public Metric(float value, float recordTime) {
            this.value = value;
            this.recordTime = recordTime;
        }

        public override string ToString() {
            return String.Format("{0, 8:F2}s: {1, 6:F2}", recordTime, value);
        }
    }
}
