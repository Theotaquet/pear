using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear {

    public class FrameRateManager : MetricsManager {

		private int framesCounter = 0;

		public FrameRateManager(MetricsManagerConfiguration metricsManager) :
				base(metricsManager) {
		}

        public override void Update() {
            framesCounter++;
        }

        public override int CalculateMetric() {
            int frameRate = (int) (framesCounter / timer);
            framesCounter = 0;
            return frameRate;
        }
    }
}
