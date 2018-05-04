using System;

namespace Pear {

    public class GarbageCollectionGen2Manager : MetricsManager {

        private int lastTotalGcCount { get; set; } = 0;

        public GarbageCollectionGen2Manager(MetricsManagerConfiguration metricsManager) :
                base(metricsManager) {
        }

        public override float CalculateMetric() {
            int totalGcCount = GC.CollectionCount(2);
            int GcCount = totalGcCount - lastTotalGcCount;
            lastTotalGcCount = totalGcCount;
            return GcCount;
        }
    }
}
