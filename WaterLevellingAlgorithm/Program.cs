using System;
using WaterLevellingAlgorithm.Alg;


namespace WaterLevellingAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            DataModeler dataModeler = new DataModeler();
            dataModeler.ReadDataSets("input.txt");
            dataModeler.PrintWaterLevels();
            Console.ReadKey();
            /* Input example (input.txt in bin\Debug\netcoreapp3.1):
                   2
                   4
                   cub 11 7 5 1
                   cub 15 6 2 2
                   cub 5 8 5 1
                   cub 19 4 8 1
                   78
                   4
                   cub 11 7 5 1
                   con 5 10 4
                   sph 7 12
                   cyl 2 4 5
                   500
            */
        }
    }
}
