using System.Runtime.Serialization;

namespace Pear {

    [DataContract(Name = "FrameRateManager")]
    public class FrameRateManager : MetricsManager {

        public int framesCounter { get; set; } = 0;

		public FrameRateManager(MetricsManagerConfiguration metricsManager) :
				base(metricsManager) {
		}

        public override void Update() {
            this.framesCounter++;
        }

        public override int CalculateMetric() {
            int frameRate = (int) (this.framesCounter / this.timer);

            if(this.timer - this.updateFrequency < this.updateFrequency) {
                this.framesCounter = 0;
            }
            return frameRate;
        }
    }
}
