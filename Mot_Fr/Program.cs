using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mot_Fr
{
    internal class Program
    {
        //test
        static void Main(string[] args)
        {
            Plateau p = new Plateau("Lettre.txt");
            Console.WriteLine(p.ToString());
            Console.ReadKey();
        }
    }
}
