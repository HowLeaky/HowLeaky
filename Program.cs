
using System;

namespace HowLeaky
{
    class Program
    {
        static void Main(string[] args)
        {
            //Project p = new Project(@"C:\projects\HLTest\Banana_reeftool.hlk");

            //Project p = new Project(@"C:\projects\HLTest\Compare\Model test.hlk");
            //p.RunSimulations()

            Project p = null;
            int numberOfCores = -1; 
            if(args.Length > 0)
            {
                p = new Project(args[0].ToString());
            }

            if(args.Length == 2)
            {
                numberOfCores = int.Parse(args[1].ToString());
            }

            p.RunSimulations(numberOfCores);

            Console.WriteLine("Press any key to exit when sims completed...\n");
            Console.ReadKey();
        }
    }
}
