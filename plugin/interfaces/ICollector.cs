using UnityEngine;

namespace Pear {

    public interface ICollector {

        void CollectMetrics();
        void Update();
        int CalculateMetric();
    }
}
