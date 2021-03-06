﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Device.Gpio;

namespace TrafficLightLib
{
  public abstract class TrafficLightControllerBase : IDisposable
  {
    public delegate void TrafficLightStatusMessageDelegate(object sender, TrafficLightStatusMessageArgs args);
    public TrafficLightStatusMessageDelegate TrafficLightStatusMessageHandler { get; set; }

    public TrafficLight TrafficLight { get; }

    public Guid ControllerGuid { get; }

    public TrafficPhase CurrentPhase { get; protected set; }

    protected GpioController controller;

    protected bool isDisposed = false;

    public TrafficLightControllerBase(TrafficLight trafficLight, TrafficPhase initialPhase)
    {
      TrafficLight = trafficLight;
      ControllerGuid = Guid.NewGuid();
      controller = new GpioController();
      SetupPin();
      SwitchPhase(initialPhase);
    }

    public virtual void SwitchPhase(TrafficPhase trafficPhase)
    {
      if (isDisposed)
      {
        return;
      }
      CurrentPhase = trafficPhase;
      switch (trafficPhase)
      {
        case TrafficPhase.RedPhase:
          TrafficLight.RedSignal.Status = SetPinValue(TrafficLight.RedSignal.Pin, PinValue.High);
          TrafficLight.YellowSignal.Status = SetPinValue(TrafficLight.YellowSignal.Pin, PinValue.Low);
          break;
        case TrafficPhase.RedYellowPhase:
          TrafficLight.RedSignal.Status = SetPinValue(TrafficLight.RedSignal.Pin, PinValue.High);
          TrafficLight.YellowSignal.Status = SetPinValue(TrafficLight.YellowSignal.Pin, PinValue.High);
          break;
        case TrafficPhase.YellowPhase:
          TrafficLight.YellowSignal.Status = SetPinValue(TrafficLight.YellowSignal.Pin, PinValue.High);
          TrafficLight.GreenSignal.Status = SetPinValue(TrafficLight.GreenSignal.Pin, PinValue.Low);
          break;
        case TrafficPhase.GreenPhase:
          TrafficLight.RedSignal.Status = SetPinValue(TrafficLight.RedSignal.Pin, PinValue.Low);
          TrafficLight.YellowSignal.Status = SetPinValue(TrafficLight.YellowSignal.Pin, PinValue.Low);
          TrafficLight.GreenSignal.Status = SetPinValue(TrafficLight.GreenSignal.Pin, PinValue.High);
          break;
      }
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

    protected virtual void SetupPin()
    {
      controller.OpenPin(TrafficLight.RedSignal.Pin, PinMode.Output);
      controller.OpenPin(TrafficLight.YellowSignal.Pin, PinMode.Output);
      controller.OpenPin(TrafficLight.GreenSignal.Pin, PinMode.Output);
    }

    #region Dispose
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
