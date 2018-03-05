using System;

namespace Pear {

    [Serializable]
    public class Metric {

        public string Type {get; set;}
        public int Val {get; set;}
        public float RecordTime {get; set;}

        public Metric(string type, int val, float recordTime) {
            this.Type = type;
            this.Val = val;
            this.RecordTime = recordTime;
        }

        public override string ToString() {
            return String.Format("{0, 8:F2}s: {1, 3}", RecordTime, Val);
        }
    }
}
