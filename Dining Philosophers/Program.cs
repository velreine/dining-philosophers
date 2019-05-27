using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dining_Philosophers
{
    class Program
    {
        private static readonly object[] Forks = { false, false, false, false, false };
        private static readonly Philosopher[] Philosophers = new Philosopher[5];
        private static ConsoleWriter cw;

        static void Main(string[] args)
        {
            Random rng = new Random();

            cw = new ConsoleWriter(Forks);



            for (int i = 0; i < 5; i++)
            {
                Philosophers[i] = new Philosopher($"Philosopher_{i}", i);
                int temp = i;
                Thread t = new Thread(() => PhilosopherWork(Philosophers[temp], rng));

                t.Start();
            }

            Console.ReadLine();


        }

        private static void PhilosopherWork(Philosopher phil, Random rng)
        {
            //Console.WriteLine($"Philosopher: {phil.Name} entered its loop.");

            while (true)
            {
                //cw.PrintHeader();

                // Try to Grab fork to the left and right.
                var leftFork = (phil.LocationAtTable - 1) == -1 ? Forks.Length - 1 : (phil.LocationAtTable % Forks.Length);
                var rightFork = phil.LocationAtTable;
                bool ateThisCycle = false;

                try
                {

                    // Try to grab the left first.
                    if (Monitor.TryEnter(Forks[leftFork]))
                    {
                        // Try to grab the right also.
                        if (Monitor.TryEnter(Forks[rightFork]))
                        {
                            phil.Eat();

                            ateThisCycle = true;
                        }
                        else
                        {
                            Monitor.Exit(Forks[leftFork]);
                        }
                    }

                    if (!ateThisCycle && Monitor.TryEnter(Forks[rightFork]))
                    {
                        if(Monitor.TryEnter(Forks[leftFork]))
                        {
                            phil.Eat();
                        }
                        else
                        {
                            Monitor.Exit(Forks[rightFork]);
                        }
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    if (Monitor.IsEntered(Forks[leftFork]))
                    {
                        Monitor.Exit(Forks[leftFork]);
                    }

                    if (Monitor.IsEntered(Forks[rightFork]))
                    {
                        Monitor.Exit(Forks[rightFork]);
                    }

                }

                // Only works when this random sleep is introduced.
                Thread.Sleep(rng.Next(1000, 1500));
            }
        }

    }
}
