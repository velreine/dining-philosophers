﻿using System;
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
        private static object[] _forks = {false, false, false, false, false};
        private static readonly Philosopher[] Philosophers = new Philosopher[5];


        static void Main(string[] args)
        {
            Random rng = new Random();

            for (int i = 0; i < 5; i++)
            {
                Philosophers[i] = new Philosopher($"Philosopher_{i}", i);
                Thread t = new Thread(PhilosopherWork);

                object argsX = new object[2] { Philosophers[i], rng};

                //t.Start(Philosophers[i]);
                t.Start(argsX);
               
            }

            Console.ReadLine();


        }

        private static void PhilosopherWork(object args)
        {

            Array argArray = new object[3];
            argArray = (Array) args;

            Philosopher phil = (Philosopher)argArray.GetValue(0);
            Random rng = (Random)argArray.GetValue(1);

            Console.WriteLine($"Philosopher: {phil.Name} entered its loop.");
            
            while (true)
            {
                // Try to Grab fork to the left and right.
                try
                {
                    // If we can grab the left then.
                    if (Monitor.TryEnter(_forks[phil.LocationAtTable], rng.Next(100, 1000)))
                    {
                        // Try to grab the right also.
                        phil.Wait();
                        if (Monitor.TryEnter(_forks[phil.LocationAtTable + 1], 1000))
                        {
                            phil.Eat();                            
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
                    if (Monitor.IsEntered(_forks[phil.LocationAtTable]))
                    {
                        Monitor.Exit(_forks[phil.LocationAtTable]);
                    }

                    if (Monitor.IsEntered(_forks[phil.LocationAtTable + 1]))
                    {
                        Monitor.Exit(_forks[phil.LocationAtTable + 1]);
                    }

                    phil.Think();
                }
                //Thread.Sleep(rng.Next(100, 1500));
            }
        }

    }
}