using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace TrafficLightLib
{
    /// <summary>
    /// Controller class for a TrafficLight
    /// </summary>
    public sealed class TrafficLightController : ATrafficLightController
    {
        public delegate void TrafficLightStatusMessageHandler(object sender, TrafficLightStatusMessageArgs args);
        public event TrafficLightStatusMessageHandler TrafficLightStatusMessageEvent;
       
        private CancellationToken cancellationToken;
        private System.Timers.Timer timer;
        

        /// <summary>
        /// Createas a new Trafficlight controller
        /// </summary>
        /// <param name="trafficLight">The traffic light to controll</param>
        /// <param name="initialPhase">The initial phase for this traffic light</param>
        public TrafficLightController(TrafficLight trafficLight, TrafficPhase initialPhase)
            :base(trafficLight, initialPhase)
        {
            
        }

        public Task<int> StartTrafficLight(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
            this.cancellationToken.Register(() => {
                timer.Stop();
            });
            return Task.Run(() => {
                TrafficLoop();
                return 1;
            });
        }

        private void TrafficLoop() 
        {
            timer = new System.Timers.Timer(5000);
            timer.Elapsed += (object send, System.Timers.ElapsedEventArgs args) =>
            {
                switch (CurrentPhase)
                {
                    case TrafficPhase.RedPhase:
                        SwitchPhase(TrafficPhase.RedYellowPhase);
                        SendStatusMessage("Activate redsignal and yellowsignal", TrafficPhase.RedYellowPhase);
                        timer.Interval = 2000;
                        break;
                    case TrafficPhase.RedYellowPhase:
                        SwitchPhase(TrafficPhase.GreenPhase);
                        SendStatusMessage("Activate greensignal", TrafficPhase.GreenPhase);
                        timer.Interval = 10000;
                        break;
                    case TrafficPhase.YellowPhase:
                        SwitchPhase(TrafficPhase.RedPhase);
                        SendStatusMessage("Activate redsignal", TrafficPhase.RedPhase);
                        timer.Interval = 5000;
                        break;
                    case TrafficPhase.GreenPhase:
                        SwitchPhase(TrafficPhase.YellowPhase);
                        SendStatusMessage("Activate yellowsignal", TrafficPhase.YellowPhase);
                        timer.Interval = 2000;
                        break;
                    default:
                        break;
                }
            };
            timer.Start();
        }

        private void SendStatusMessage(string message, TrafficPhase phase) 
        {
            TrafficLightStatusMessageEvent?.Invoke(this, new TrafficLightStatusMessageArgs(message, phase, ControllerGuid));
        }
        
        protected override void Dispose(bool isDisposing) 
        {
            if(!isDisposed) 
            {
                if(isDisposing) 
                {
                    timer.Dispose();
                    timer = null;
                }
                isDisposed = true;
            }
            base.Dispose(isDisposing);
        }
    }
}
