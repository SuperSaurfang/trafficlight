namespace TrafficLightLib
{

    /// <summary>
    /// Summary class for three Trafficsignals, one for a green signal, one for a yellow signal and one for a red signal
    /// </summary>
    public class TrafficLight
    {
        /// <summary>
        /// Creates a new Trafficlight, with the specified pin numbers
        /// </summary>
        /// <param name="redPin">the pin number for the red led</param>
        /// <param name="yellowPin">the pin number for the yellow led</param>
        /// <param name="greenPin">the pin number for the green led</param>
      public TrafficLight(int redPin, int yellowPin, int greenPin)
      {
        RedSignal = new Signal(SignalColor.Red, false, redPin);

        YellowSignal = new Signal(SignalColor.Yellow, false, yellowPin);

        GreenSignal = new Signal(SignalColor.Green, false, greenPin);
      }
      public Signal RedSignal { get; }
      public Signal YellowSignal { get; }
      public Signal GreenSignal { get; }
    }
}
