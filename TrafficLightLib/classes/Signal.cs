namespace TrafficLightLib
{
    /// <summary>
    /// Signal class to represent a Signal, the TrafficLight struct is build on this struct
    /// </summary>
    public class Signal
    {
        /// <summary>
        /// Creates a new Signal, with the specified parameters
        /// </summary>
        /// <param name="color">The color of the signal</param>
        /// <param name="status">The current status of the Signal</param>
        /// <param name="pin">The pin used by this signal</param>
        public Signal(SignalColor color, bool status, int pin) {
            Color = color;
            Status = status;
            Pin = pin;
        }

        public SignalColor Color { get; }
        /// <summary>
        /// Don't change the status by yourself to prevent issues!
        /// </summary>
        public bool Status { get; set; }
        public int Pin { get;  }
    }
}
