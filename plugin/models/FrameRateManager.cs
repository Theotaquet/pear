using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear {

    public class FrameRateManager : MetricsManager {

		public FrameRateManager(string name, bool enabled, float updateFrequency) :
				base(name, enabled, updateFrequency) {
		}

		public FrameRateManager(MetricsManagerConfiguration metricsManager) :
				base(metricsManager) {
		}
        public override void CollectMetrics() {
        }
    }
}
