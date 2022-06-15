using System;
using System.ServiceModel;

namespace GameHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("wait while GAME SERVER is starting......");
            ServiceHost host = new ServiceHost(typeof(GameService.GameService));

            host.Open();

            Console.WriteLine("The Game Server is ON");
            Console.ReadLine();

            host.Close();
            Console.WriteLine("The Game Server is OFF");
        }
    }
}
