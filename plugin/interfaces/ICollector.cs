using UnityEngine;

namespace Pear {

    public interface ICollector {

        void CollectMetrics(float lastFrameTime);
    }
}
