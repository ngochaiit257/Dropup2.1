using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestEndCode;

namespace Test_Function
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type password: ");
            string pass = Console.ReadLine();
            pass = EndCode.Encrypt(pass);
            Console.WriteLine("Password encrypt: " + pass);
            Console.ReadKey();
        }
    }
}
