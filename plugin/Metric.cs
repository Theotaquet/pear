using System;
using UnityEngine;

namespace AssemblyCSharp {

    //performance data recorded during a session
    [Serializable]
    public class Metric {

        //the type of metric (fps, frametime, ...)
        public string type;
        //the recorded value
        public int value;
        //the time the metric was recorded
        public uint time;

        public Metric(string type, int value, uint time) {
            this.type = type;
            this.value = value;
            this.time = time;
        }

        public override string ToString() {
            return time + " - " + type + ": " + value;
        }
    }
}
