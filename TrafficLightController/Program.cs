using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrafficLightLib;

namespace TrafficLightControllerExec
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TrafficLight trafficLight = new TrafficLight(4, 3, 2);
            TrafficLight trafficLight1 = new TrafficLight(18, 15, 14);
            TrafficLight trafficLight2 = new TrafficLight(22, 27, 17);
            TrafficLight trafficLight3 = new TrafficLight(11, 9, 10);

            PedestrainLight pedestrainLight = new PedestrainLight(23, 24, 20);

            Console.WriteLine("Welcome to traffic light controller");

            using (var ped = new PedestrainLightController(pedestrainLight))
            {
                Console.WriteLine("Waiting");
                Console.ReadKey();
            }

            /*using(var trafficLightController = new TrafficLightController(trafficLight3, TrafficPhase.RedPhase))
            {
                trafficLightController.TrafficLightStatusMessageHandler = MessageHandler;
                Console.WriteLine("Initial Staus:");
                PrintStatus(trafficLightController);
                Console.WriteLine("Start task");
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var task = trafficLightController.StartTrafficLight(tokenSource.Token);

                Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs args) =>
                {
                    tokenSource.Cancel();
                    task.Wait();

                    Console.WriteLine($"Task completed: {task.IsCompleted}");
                    Console.WriteLine("Bye");
                };

                Console.WriteLine($"Task completed: {task.IsCompleted}");
                var result = await task;
                Console.WriteLine($"Result: {result}");
                Console.ReadKey();
            }*/
            Console.WriteLine("Bye?");
            Console.ReadKey();
        }

        private static void MessageHandler(object sender, TrafficLightStatusMessageArgs args)
        {
            Console.WriteLine($"Controller: {args.ControllerGuid} send message: {args.Message} and has phase: {args.TrafficPhase}");
        }

        private static void PrintStatus(TrafficLightController controller)
        {
            Console.WriteLine($"Redsignal: {controller.TrafficLight.RedSignal.Status}");
            Console.WriteLine($"Yellowsignal: {controller.TrafficLight.YellowSignal.Status}");
            Console.WriteLine($"Greensignal: {controller.TrafficLight.GreenSignal.Status}");
        }
    }
}
