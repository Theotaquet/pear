
namespace Pear {

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
