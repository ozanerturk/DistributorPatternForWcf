using System;

namespace DistributorPatternForWcf
{

    class Program
    {
        static void Main(string[] args)
        {
            DistibutorPattern();
        }

        static void DistibutorPattern()
        {
            var distributor = new Distributor();

            for (int i = 0; i < 100000; i++)
            {
                distributor.ExecuteNext(client =>
                {
                    Console.WriteLine("Distributor: " + client.EndpointName);
                });
            }
            Console.ReadLine();
        }

    }
}
