using System;
using System.Collections.Generic;
using System.Device.Gpio;

namespace TrafficLightLib
{
    public struct Signal 
    {
        public Signal(SignalColor color, SignalStatus status, int pin) {
            this.color = color;
            this.status = status;
            this.pin = pin;
        }

        private readonly SignalColor color;
        private readonly SignalStatus status;
        private readonly int pin;

        public SignalColor Color { get { return color; } }
        public SignalStatus Status { get { return status; } }
        public int Pin { get { return pin; } }
    }

    public class TrafficLight : IDisposable
    {
        private readonly List<Signal> signals;
        private readonly GpioController controller;
        public TrafficLight(List<Signal> signals, TrafficPhase initialPhase) {
            if(signals.Count > 4) 
            {
                throw new ArgumentException($"3 Signals are allowed. It was {signals.Count + 1} Signals");
            }
            if(!CheckSignals(signals)) {
                throw new ArgumentException("Invalid Signals. Add one green, one yellow one red signal");
            }

            this.signals = signals;
            this.controller = new GpioController();

            this.SetupPins();
            this.SwitchPhase(initialPhase);
        }

        public void SwitchPhase(TrafficPhase trafficPhase) {
            var redSignal = this.signals.Find(signal => signal.Color == SignalColor.Red); 
            var yellowSignal = this.signals.Find(signal => signal.Color == SignalColor.Yellow); 
            var greenSignal = this.signals.Find(signal => signal.Color == SignalColor.Green); 
            switch (trafficPhase)
            {
                case TrafficPhase.RedPhase:
                    this.controller.Write(redSignal.Pin, PinValue.High);
                    this.controller.Write(yellowSignal.Pin, PinValue.Low);
                    return;
                case TrafficPhase.RedYellowPhase:
                    this.controller.Write(redSignal.Pin, PinValue.High);
                    this.controller.Write(yellowSignal.Pin, PinValue.High);
                    return;
                case TrafficPhase.YellowPhase:
                    this.controller.Write(yellowSignal.Pin, PinValue.High);
                    this.controller.Write(greenSignal.Pin, PinValue.Low);
                    return;
                case TrafficPhase.GreenPhase:
                    this.controller.Write(redSignal.Pin, PinValue.Low);
                    this.controller.Write(yellowSignal.Pin, PinValue.Low);
                    this.controller.Write(greenSignal.Pin, PinValue.High);
                    return;
            }
        }

        private bool CheckSignals(List<Signal> signals) {
            bool isRedSignal = false;
            bool isYellowSignal = false;
            bool isGreenSignal = false;
            foreach (var signal in signals)
            {
                switch (signal.Color)
                {
                    case SignalColor.Green:
                        isGreenSignal = true;
                        break;
                    case SignalColor.Yellow:
                        isYellowSignal = true;
                        break;
                    case SignalColor.Red:
                        isRedSignal = true;
                        break;
                }
            }

            return isRedSignal && isYellowSignal && isGreenSignal;
        }

        private void SetupPins() {
            foreach (var signal in signals)
            {
                this.controller.OpenPin(signal.Pin, PinMode.Output);
            }
        }

        public void Dispose()
        {
            this.controller.Dispose();
        }
    }
}
