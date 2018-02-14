using System;
using UnityEngine;

namespace Pear {

    [Serializable]
    public class Metric {

        public string type;
        public int val;
        public uint recordTime;

        public Metric(string type, int val, uint recordTime) {
            this.type = type;
            this.val = val;
            this.recordTime = recordTime;
        }

        public override string ToString() {
            return recordTime + " - " + type + ": " + val;
        }
    }
}
