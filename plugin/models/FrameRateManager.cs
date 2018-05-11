using System.Runtime.Serialization;

namespace Pear {

    public class FrameRateManager : MetricsManager {

        public int framesCounter { get; set; } = 0;

        public FrameRateManager(MetricsManagerConfiguration metricsManager) :
                base(metricsManager) {
        }

        public override void Update() {
            framesCounter++;
        }

        public override float CalculateMetric() {
            float frameRate = framesCounter / timer;

            if(timer - updateFrequency < updateFrequency) {
                framesCounter = 0;
            }
            return frameRate;
        }
    }
}
