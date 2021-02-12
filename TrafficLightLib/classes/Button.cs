using System;
using System.Collections.Generic;
using System.Text;

namespace TrafficLightLib
{
    public class Button
    {
        public Button(int pin) 
        {
            IsPressed = false;
            Pin = pin;
        }

        public bool  IsPressed { get; set; }

        public int Pin { get; }

    }
}
