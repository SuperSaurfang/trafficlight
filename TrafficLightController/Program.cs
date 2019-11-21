using System;
using System.Collections.Generic;
using System.Threading;
using TrafficLightLib;

namespace TrafficLightController
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Signal> signals = new List<Signal>() {
                new Signal(SignalColor.Red, SignalStatus.Off, 4),
                new Signal(SignalColor.Yellow, SignalStatus.Off, 3),
                new Signal(SignalColor.Green, SignalStatus.Off, 2),
            };

            Console.WriteLine("Welcome to traffic light controller");

            using(TrafficLight trafficLight = new TrafficLight(signals, TrafficPhase.RedPhase))
            {
                bool isRunning = true;

                Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs args) => 
                {
                    Console.WriteLine("Last Round");
                    trafficLight.Dispose();
                };

                while(isRunning) 
                {
                    Thread.Sleep(5000);
                    trafficLight.SwitchPhase(TrafficPhase.RedYellowPhase);
                    Thread.Sleep(2000);
                    trafficLight.SwitchPhase(TrafficPhase.GreenPhase);
                    Thread.Sleep(10000);
                    trafficLight.SwitchPhase(TrafficPhase.YellowPhase);
                    Thread.Sleep(2000);
                    trafficLight.SwitchPhase(TrafficPhase.RedPhase);
                }
                
            }
            Console.WriteLine("Bye!");
        }
    }
}
