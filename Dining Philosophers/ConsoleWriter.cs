using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dining_Philosophers
{
    class ConsoleWriter
    {
        public string Header { get; private set; }
        private StringBuilder sb = new StringBuilder();
        private object[] forks;

        public ConsoleWriter(object[] forks)
        {
            this.forks = forks;
        }

        public void Print(string msg)
        {
            Console.Clear();
            PrintHeader();
            Append(msg);
            Console.WriteLine(sb.ToString());
        }

        public void Append(string msg)
        {
            sb.Append(msg);

        }

        public void PrintHeader()
        {
            foreach (var obj in forks)
            {
                Console.Write((bool)obj);
                Console.WriteLine();
            }
        }

    }
}
