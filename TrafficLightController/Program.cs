using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrafficLightLib;

namespace TrafficLightControllerExec
{
    class Program
    {
        static void Main(string[] args)
        {
            TrafficLight westLight = new TrafficLight(4, 3, 2);
            TrafficLight northLight = new TrafficLight(18, 15, 14);
            TrafficLight eastLight = new TrafficLight(22, 27, 17);
            TrafficLight southLight = new TrafficLight(11, 9, 10);

            Console.WriteLine("Welcome to traffic light controller");

            using(var westLightController = new TrafficLightController(westLight, TrafficPhase.RedPhase))
            using(var northLightController = new TrafficLightController(northLight, TrafficPhase.RedPhase))
            using(var eastLightController = new TrafficLightController(eastLight, TrafficPhase.RedPhase))
            using(var southLightController = new TrafficLightController(southLight, TrafficPhase.RedPhase))
            {
                var machine = new TrafficLightMachine
                {
                    MachineStateEventHandler = MessageHandler
                };
                machine.AddTrafficLightController(westLightController, CardinalPoint.West);
                machine.AddTrafficLightController(northLightController, CardinalPoint.North);
                machine.AddTrafficLightController(eastLightController, CardinalPoint.East);
                machine.AddTrafficLightController(southLightController, CardinalPoint.South);

                machine.Start();
                Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs args) =>
                {
                    machine.Stop();
                    Console.WriteLine("Bye");
                };
                Console.ReadKey();
            }
            Console.WriteLine("Bye?");
            Console.ReadKey();
        }

        private static void MessageHandler(object sender, MachineStateEventArgs args)
        {
            Console.WriteLine($"Controller: {args.ControllerGuid} send message: {args.Message} and has phase: {args.TrafficPhase}");
        }
    }
}
