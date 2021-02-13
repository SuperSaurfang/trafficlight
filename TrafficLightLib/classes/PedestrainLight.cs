using System;
using System.Collections.Generic;
using System.Text;

namespace TrafficLightLib
{
    public class PedestrainLight
    {
        public PedestrainLight(List<int> greenPins, List<int> redPins, int buttonPin)
        {
          RedSignals = new List<Signal>();
          foreach (var pin in redPins)
          {
              RedSignals.Add(new Signal(SignalColor.Red, false, pin));
          }

          GreenSignals = new List<Signal>();
          foreach (var pin in greenPins)
          {
              GreenSignals.Add(new Signal(SignalColor.Green, false, pin));
          }
          Button = new Button(buttonPin);
        }

        public List<Signal> RedSignals {get;}
        public List<Signal> GreenSignals {get;}
        public Button Button { get; }
    }
}
