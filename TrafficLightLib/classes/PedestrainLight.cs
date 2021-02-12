using System;
using System.Collections.Generic;
using System.Text;

namespace TrafficLightLib
{
    public class PedestrainLight
    {
        public PedestrainLight(int greenPin, int redPin, int buttonPin)
        {
            RedSignal = new Signal(SignalColor.Red, false, redPin);
            GreenSignal = new Signal(SignalColor.Green, false, greenPin);
            Button = new Button(buttonPin);
        }

        public Signal RedSignal { get; }
        public Signal GreenSignal { get; }
        public Button Button { get; }
    }
}
