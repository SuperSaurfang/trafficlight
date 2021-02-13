using System;
using System.Collections.Generic;
using System.Text;
using System.Device.Gpio;

namespace TrafficLightLib
{
  public abstract class PedestrainLightControllerBase : IDisposable
  {
    public delegate void PedestrainLightInterruptDelegate(object sender, PedestainLightInterruptArgs args);

    public PedestrainLightInterruptDelegate PedestrainLightInterruptHandler { get; set; }
    public PedestrainLight PedestrainLight { get; }

    public Guid ControllerGuid { get; }

    public PedestrainState CurrentState { get; protected set; }

    protected GpioController controller;
    protected bool isDisposed = false;


    public PedestrainLightControllerBase(PedestrainLight pedestrainLight)
    {
      PedestrainLight = pedestrainLight;
      ControllerGuid = Guid.NewGuid();
      controller = new GpioController();
      CurrentState = PedestrainState.Red;
      SetupPin();
      SetInitial();
    }

    public virtual void SetInitial()
    {
      foreach (var signal in PedestrainLight.RedSignals)
      {
        signal.Status = SetPinValue(signal.Pin, PinValue.High);
        controller.Write(signal.Pin, PinValue.High);
      }

      foreach (var signal in PedestrainLight.GreenSignals)
      {
        signal.Status = SetPinValue(signal.Pin, PinValue.Low);
      }
    }

    protected virtual void SetupPin()
    {
      foreach (var signal in PedestrainLight.GreenSignals)
      {
        controller.OpenPin(signal.Pin, PinMode.Output);
      }
      foreach (var signal in PedestrainLight.RedSignals)
      {
        controller.OpenPin(signal.Pin, PinMode.Output);
      }

      controller.OpenPin(PedestrainLight.Button.Pin, PinMode.Input);
      controller.RegisterCallbackForPinValueChangedEvent(PedestrainLight.Button.Pin, PinEventTypes.Rising, PinChangeEvent);
    }

    public abstract void PinChangeEvent(object sender, PinValueChangedEventArgs args);

    protected virtual void PedestainLightInterruptEvent(object sender, PedestainLightInterruptArgs args)
    {
      PedestrainLightInterruptHandler?.Invoke(sender, args);
    }
    protected virtual bool SetPinValue(int pin, PinValue value)
    {
      controller.Write(pin, value);
      if (value == PinValue.High)
      {
        return true;
      }
      return false;
    }

    #region IDisposable
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
      if (!isDisposed)
      {
        if (isDisposing)
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
