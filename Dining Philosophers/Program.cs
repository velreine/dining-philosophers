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
        private static readonly Random RNG = new Random();

        static void Main(string[] args)
        {

            for (int i = 0; i < 5; i++)
            {
                Philosophers[i] = new Philosopher($"Philosopher_{i}", i);
                int temp = i;
                Thread t = new Thread(() => PhilosopherWork(Philosophers[temp]));

                t.Start();
            }

            Console.ReadLine();
        }

        private static void PhilosopherWork(Philosopher phil)
        {
            //Console.WriteLine($"Philosopher: {phil.Name} entered its loop.");
            while (true)
            {
                // Try to Grab fork to the left and right.
                var leftFork = (phil.LocationAtTable - 1) == -1 ? Forks.Length - 1 : (phil.LocationAtTable % Forks.Length);
                var rightFork = phil.LocationAtTable;
                bool ateThisCycle = false;

                try
                {
                    // Try to grab the left first.
                    if (Monitor.TryEnter(Forks[leftFork]))
                    {
                        //Console.WriteLine($"{phil.Name} got left fork.");

                        // Try to grab the right also.
                        if (Monitor.TryEnter(Forks[rightFork]))
                        {
                            //Console.WriteLine($"{phil.Name} got right fork.");
                            phil.Eat();

                            ateThisCycle = true;
                        }
                        else
                        {
                            //Console.WriteLine($"{phil.Name} could not get right fork, releasing left.");
                            // We didn't get the right fork, so release the left one.
                            Monitor.Exit(Forks[leftFork]);
                        }
                    }

                    // If we haven't eaten in this cycle we can try to grab the right first instead.
                    if (!ateThisCycle && Monitor.TryEnter(Forks[rightFork]))
                    {
                        //Console.WriteLine($"--{phil.Name} got right fork.");
                        // And then try to grab the left.
                        if (Monitor.TryEnter(Forks[leftFork]))
                        {
                            //Console.WriteLine($"--{phil.Name} got left fork.");
                            phil.Eat();
                        }
                        else
                        {
                            //Console.WriteLine($"{phil.Name} could not get left fork, releasing right.");
                            // We didn't get the left fork, so release the right one.
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

                    if (ateThisCycle)
                        phil.Think();
                }
            }
        }
    }
}
