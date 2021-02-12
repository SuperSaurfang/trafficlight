using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Text;

namespace TrafficLightLib
{
    public sealed class PedestrainLightController : PedestrainLightControllerBase
    {
        public PedestrainLightController(PedestrainLight pedestrainLight)
            : base(pedestrainLight)
        {
        }

        public override void PinChangeEvent(object sender, PinValueChangedEventArgs args)
        {
            switch (args.ChangeType)
            {
                case PinEventTypes.Falling:
                  PedestrainLight.Button.IsPressed = false;
                  break;
                case PinEventTypes.Rising:
                  PedestrainLight.Button.IsPressed = true;
                  break;
                case PinEventTypes.None:
                default:
                  PedestrainLight.Button.IsPressed = false;
                  break;
            }
            controller.Write(PedestrainLight.GreenSignal.Pin, PinValue.High);
            controller.Write(PedestrainLight.RedSignal.Pin, PinValue.Low);
        }
    }
}
