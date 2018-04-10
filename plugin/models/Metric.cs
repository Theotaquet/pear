using System;

namespace Pear {

    [Serializable]
    public class Metric {

        public int val;
        public float recordTime;

        public Metric(int val, float recordTime) {
            this.val = val;
            this.recordTime = recordTime;
        }

        public override string ToString() {
            return String.Format("{0, 8:F2}s: {1, 3}", recordTime, val);
        }
    }
}
