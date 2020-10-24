using System;

namespace TrafficLightLib
{
    public class TrafficLightStatusMessageArgs : EventArgs
    {
        public TrafficLightStatusMessageArgs(string message, TrafficPhase phase, Guid guid)
        {
            Message = message;
            TrafficPhase = phase;
            ControllerGuid = guid;
        }
        public string Message { get; }
        public TrafficPhase TrafficPhase { get; }
        public Guid ControllerGuid {get;}
    }
}
