using System;

namespace Pear {

    [Serializable]
    public class Metric {

        public float value;
        public float recordTime;

        public Metric(float value, float recordTime) {
            this.value = value;
            this.recordTime = recordTime;
        }

        public override string ToString() {
            return String.Format("{0, 8:F2}s: {1, 6:F2}", recordTime, value);
        }
    }
}
