using System;
using System.Collections.Generic;
using System.Text;
using System.Device.Gpio;

namespace TrafficLightLib
{
    public abstract class PedestrainLightControllerBase : IDisposable
    {
        public PedestrainLight PedestrainLight { get; }

        public Guid ControllerGuid { get; }

        protected GpioController controller;
        protected bool isDisposed = false;


        public PedestrainLightControllerBase(PedestrainLight pedestrainLight)
        {
            PedestrainLight = pedestrainLight;
            ControllerGuid = Guid.NewGuid();
            controller = new GpioController();
            SetupPin();

            controller.Write(PedestrainLight.RedSignal.Pin, PinValue.High);
            controller.Write(PedestrainLight.GreenSignal.Pin, PinValue.Low);
        }

        protected virtual void SetupPin()
        {
            controller.OpenPin(PedestrainLight.RedSignal.Pin, PinMode.Output);
            controller.OpenPin(PedestrainLight.GreenSignal.Pin, PinMode.Output);
            controller.OpenPin(PedestrainLight.Button.Pin, PinMode.Input);
            controller.RegisterCallbackForPinValueChangedEvent(PedestrainLight.Button.Pin, PinEventTypes.Rising, PinChangeEvent);
        }

        public abstract void PinChangeEvent(object sender, PinValueChangedEventArgs args);

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if(!isDisposed)
            {
                if(isDisposing)
                {
                    controller.Dispose();
                    controller = null;
                }
                isDisposed = true;
            }
        }
        #endregion
    }
}
