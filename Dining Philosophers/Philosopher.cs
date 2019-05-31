using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dining_Philosophers
{
    class Philosopher
    {
        private readonly Random _rng = new Random();
        public string Name { get; set; }
        public int LocationAtTable { get; set; }

        public Philosopher(string name, int locationAtTable)
        {
            Name = name;
            LocationAtTable = LocationAtTable;
        }

        public void Eat()
        {
            Console.WriteLine($"{this.Name} is currently eating!");
            Thread.Sleep(2000);
        }

        public void Think()
        {
            Console.WriteLine($"{this.Name} is currently thinking..");
            Thread.Sleep(_rng.Next(100, 1000));
        }

        public void Wait()
        {
            Console.WriteLine($"{this.Name} is currently waiting..");
        }

    }
}
