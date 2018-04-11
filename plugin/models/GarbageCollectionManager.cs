using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear {

    public class GarbageCollectionManager : MetricsManager {

		public GarbageCollectionManager(string name, bool enabled, float updateFrequency) :
				base(name, enabled, updateFrequency) {
		}

		public GarbageCollectionManager(MetricsManagerConfiguration metricsManager) :
				base(metricsManager) {
		}
        public override void CollectMetrics() {
        }
    }
}
