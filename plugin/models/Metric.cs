using System;

namespace Pear {

    [Serializable]
    public class Metric {

        public int value;
        public float recordTime;

        public Metric(int value, float recordTime) {
            this.value = value;
            this.recordTime = recordTime;
        }

        public override string ToString() {
            return String.Format("{0, 8:F2}s: {1, 3}", recordTime, value);
        }
    }
}
