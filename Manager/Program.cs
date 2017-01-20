using System;

namespace Manager
{
    class Program
    {
        static void Main()
        {
            var manager = Manager.Instance();
            manager.Run();

            Console.WriteLine("Press any key to complete system monitoring...`");

            Console.ReadKey();

            manager.Stop();
        }
    }
}
