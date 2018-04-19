using UnityEngine;

namespace Pear {

    public interface ICollector {

        void CollectMetrics();
        int CalculateMetric();
    }
}
