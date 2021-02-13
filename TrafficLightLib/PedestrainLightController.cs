using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Text;

namespace TrafficLightLib
{
    public sealed class PedestrainLightController : PedestrainLightControllerBase
    {
      private System.Timers.Timer greenSignalTimer;
      private System.Timers.Timer redSignalTimer;
        public PedestrainLightController(PedestrainLight pedestrainLight)
            : base(pedestrainLight)
        {
          greenSignalTimer = new System.Timers.Timer(2000);
          redSignalTimer = new System.Timers.Timer(2000);

          greenSignalTimer.Elapsed += GreenPhase;
          redSignalTimer.Elapsed += RedPhase;
        }

        public override void PinChangeEvent(object sender, PinValueChangedEventArgs args)
        {
            switch (args.ChangeType)
            {
                case PinEventTypes.Rising:
                  greenSignalTimer.Start();
                  PedestainLightInterruptEvent(this, new PedestainLightInterruptArgs(ControllerGuid, "Interrupted", PedestrainState.Interruped));
                  break;
                case PinEventTypes.Falling:
                case PinEventTypes.None:
                default:
                  break;
            }
        }

        public void GreenPhase(object send, System.Timers.ElapsedEventArgs args)
        {
          CurrentState = PedestrainState.Green;
          PedestainLightInterruptEvent(this, new PedestainLightInterruptArgs(ControllerGuid, "Green", CurrentState));
          foreach (var signal in PedestrainLight.GreenSignals)
          {
            signal.Status = SetPinValue(signal.Pin, PinValue.High);
          }
          foreach (var signal in PedestrainLight.RedSignals)
          {
            signal.Status = SetPinValue(signal.Pin, PinValue.Low);
          }
          greenSignalTimer.Stop();
          redSignalTimer.Start();
        }

        public void RedPhase(object send, System.Timers.ElapsedEventArgs args)
        {
          CurrentState = PedestrainState.Red;
          PedestainLightInterruptEvent(this, new PedestainLightInterruptArgs(ControllerGuid, "Red", CurrentState));
          foreach (var signal in PedestrainLight.GreenSignals)
          {
            signal.Status = SetPinValue(signal.Pin, PinValue.Low);
          }
          foreach (var signal in PedestrainLight.RedSignals)
          {
            signal.Status = SetPinValue(signal.Pin, PinValue.High);
          }
          redSignalTimer.Stop();
        }
    }
}
