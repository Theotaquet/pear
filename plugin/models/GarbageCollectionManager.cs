using System;
using System.Runtime.Serialization;

namespace Pear {

    [DataContract(Name = "GarbageCollectionManager")]
    public class GarbageCollectionManager : MetricsManager {

		public GarbageCollectionManager(MetricsManagerConfiguration metricsManager) :
				base(metricsManager) {
		}

        public override int CalculateMetric() {
            int GCCount = GC.CollectionCount(0);
            return GCCount;
        }
    }
}
