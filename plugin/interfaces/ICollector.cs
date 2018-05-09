namespace Pear {

    public interface ICollector {

        void CollectMetrics();
        void Update();
        float CalculateMetric();
    }
}
