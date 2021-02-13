using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace TrafficLightLib
{
    public class MachineStateEventArgs 
    {
        public MachineStateEventArgs(string message, TrafficPhase phase, Guid guid)
        {
            Message = message;
            TrafficPhase = phase;
            ControllerGuid = guid;
        }
        public string Message { get; }
        public TrafficPhase TrafficPhase { get; }
        public Guid ControllerGuid { get; }
    }
    public class TrafficLightMachine
    {
        public delegate void MachineStateEvent(object sender, MachineStateEventArgs args);
        public MachineStateEvent MachineStateEventHandler;
        private readonly Dictionary<CardinalPoint, TrafficLightControllerBase> trafficLightControllers;
        private readonly Dictionary<CardinalPoint, TrafficPhase> currentPhase;
        private readonly System.Timers.Timer timer = new System.Timers.Timer(5000);

        public TrafficLightMachine() 
        {
            trafficLightControllers = new Dictionary<CardinalPoint, TrafficLightControllerBase>();
            currentPhase = new Dictionary<CardinalPoint, TrafficPhase>()
            {
                { CardinalPoint.North, TrafficPhase.RedPhase },
                { CardinalPoint.East, TrafficPhase.RedPhase },
                { CardinalPoint.South, TrafficPhase.RedPhase },
                { CardinalPoint.West, TrafficPhase.RedPhase }
            };
            timer = new System.Timers.Timer(5000);
            timer.Elapsed += ElapsedEvent;
        }

        public void AddTrafficLightController(TrafficLightControllerBase trafficLightController, CardinalPoint cardinalPoint) 
        {
            //chek if key is already there
            if(trafficLightControllers.ContainsKey(cardinalPoint)) 
            {
                return;
            }

            //check if an item has the same guid as the new item
            var item = trafficLightControllers.FirstOrDefault(p => p.Value.ControllerGuid == trafficLightController.ControllerGuid).Value;
            if (item != null) 
            {
                return;
            }

            trafficLightControllers.Add(cardinalPoint, trafficLightController);
            currentPhase[cardinalPoint] = trafficLightController.CurrentPhase;
        }

        public void Start() 
        {
            if(trafficLightControllers.Count == 0) 
            {
                return;
            }

            timer.Start();
        }

        public void Stop() 
        {
            timer.Stop();
        }

        public Dictionary<CardinalPoint, TrafficPhase> GetCurrentPhase() 
        {
            return currentPhase;
        }

        public void NextPhase() 
        {
            
        }

        private void StateTask() 
        {
            timer.Start();
        }

        private void ElapsedEvent(object send, System.Timers.ElapsedEventArgs args)
        {
            var keys = currentPhase.Keys;
            Console.WriteLine($"Key count: {keys.Count}");
            foreach (var key in keys)
            {
                Console.WriteLine($"Current Key: {key}");
                var phase = currentPhase[key];
                var controller = trafficLightControllers[key];
                Console.WriteLine($"Phase: {phase}");
                Console.WriteLine($"Controller: {controller.ControllerGuid}");
                switch (phase)
                {
                    case TrafficPhase.RedPhase:
                        Console.WriteLine("Hello there");
                        currentPhase[key] = controller.SwitchPhase(TrafficPhase.RedYellowPhase);
                        MachineStateEventHandler?.Invoke(this, new MachineStateEventArgs("Activate redsignal and yellowsignal", currentPhase[key], controller.ControllerGuid));
                        timer.Interval = 2000;
                        break;
                    case TrafficPhase.RedYellowPhase:
                        currentPhase[key] = controller.SwitchPhase(TrafficPhase.GreenPhase);
                        MachineStateEventHandler?.Invoke(this, new MachineStateEventArgs("Activate greensignal", currentPhase[key], controller.ControllerGuid));
                        timer.Interval = 10000;
                        break;
                    case TrafficPhase.YellowPhase:
                        currentPhase[key] = controller.SwitchPhase(TrafficPhase.RedPhase);
                        MachineStateEventHandler?.Invoke(this, new MachineStateEventArgs("Activate redsignal", currentPhase[key], controller.ControllerGuid));
                        timer.Interval = 5000;
                        break;
                    case TrafficPhase.GreenPhase:
                        currentPhase[key] = controller.SwitchPhase(TrafficPhase.YellowPhase);
                        MachineStateEventHandler?.Invoke(this, new MachineStateEventArgs("Activate yellowsignal", currentPhase[key], controller.ControllerGuid));
                        timer.Interval = 2000;
                        break;
                    default:
                        Console.WriteLine($"Undefined: {key} or {phase}");
                        break;
                }
            }
        }
    }
}
