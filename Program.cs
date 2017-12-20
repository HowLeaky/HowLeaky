
using System;

namespace HowLeaky
{
    class Program
    {
        static void Main(string[] args)
        {
            Project p = new Project(@"C:\projects\HLTest\Banana_reeftool.hlk");

            Console.ReadKey();
        }
    }
}
