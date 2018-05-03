using System;

namespace Pear {

    public class GarbageCollectionGen2Manager : MetricsManager {

        public GarbageCollectionGen2Manager(MetricsManagerConfiguration metricsManager) :
                base(metricsManager) {
        }

        public override int CalculateMetric() {
            int GCCount = GC.CollectionCount(2);
            return GCCount;
        }
    }
}
