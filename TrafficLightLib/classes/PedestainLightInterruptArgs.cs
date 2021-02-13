using System;

namespace TrafficLightLib
{
  public class PedestainLightInterruptArgs
  {
    public string Message { get; }
    public PedestrainState State { get; }
    public Guid ControllerGuid { get; }
    public PedestainLightInterruptArgs(Guid guid, string message, PedestrainState state)
    {
      ControllerGuid = guid;
      Message = message;
      State = state;
    }
  }
}