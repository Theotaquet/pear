using System;

namespace Pear {

    public class GarbageCollectionGeneration1Manager : MetricsManager {

        private int lastTotalGcCount { get; set; } = 0;

        public GarbageCollectionGeneration1Manager(MetricsManagerConfiguration metricsManager) :
                base(metricsManager) {
        }

        public override float CalculateMetric() {
            int totalGcCount = GC.CollectionCount(1);
            int GcCount = totalGcCount - lastTotalGcCount;
            lastTotalGcCount = totalGcCount;
            return GcCount;
        }
    }
}
