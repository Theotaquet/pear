using System;
using UnityEngine;

namespace Pear {

    [Serializable]
    public class Metric {

        public string type;
        public int val;
        public float recordTime;

        public Metric(string type, int val, float recordTime) {
            this.type = type;
            this.val = val;
            this.recordTime = recordTime;
        }

        public override string ToString() {
            return String.Format("{0, 8:F2}s: {1, 3}", recordTime, val);
        }
    }
}
