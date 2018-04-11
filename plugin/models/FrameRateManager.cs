using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear {

    public class FrameRateManager : MetricsManager {

		private int framesCounter;

		public FrameRateManager(string name, bool enabled, float updateFrequency) :
				base(name, enabled, updateFrequency) {
			framesCounter = 0;
		}

		public FrameRateManager(MetricsManagerConfiguration metricsManager) :
				base(metricsManager) {
			framesCounter = 0;
		}
        public override void CollectMetrics(float lastFrameTime) {
			base.CollectMetrics(lastFrameTime);
			int frameRate;
            framesCounter++;

            //test if the limit of updates per second is respected
            while(timer > updateFrequency) {
                frameRate = (int) (framesCounter / timer);
                CreateMetric(new Metric(frameRate, lastFrameTime));

                //the overflow is kept in memory
                //if frameRateTimer has exceeded updateFrequency
                timer -= updateFrequency;
                framesCounter = 0;
            }
        }
    }
}
