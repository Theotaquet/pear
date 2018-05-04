using System;

namespace Pear {

    public class GarbageCollectionGen1Manager : MetricsManager {

        private int lastTotalGcCount { get; set; } = 0;

        public GarbageCollectionGen1Manager(MetricsManagerConfiguration metricsManager) :
                base(metricsManager) {
        }

        public override int CalculateMetric() {
            int totalGcCount = GC.CollectionCount(1);
            int GcCount = totalGcCount - lastTotalGcCount;
            lastTotalGcCount = totalGcCount;
            return GcCount;
        }
    }
}
