using System;

namespace Pear {

    public class GarbageCollectionGen1Manager : MetricsManager {

		public GarbageCollectionGen1Manager(MetricsManagerConfiguration metricsManager) :
				base(metricsManager) {
		}

        public override int CalculateMetric() {
            int GCCount = GC.CollectionCount(0);
            return GCCount;
        }
    }
}
