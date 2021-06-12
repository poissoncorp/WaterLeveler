using System;
using System.IO;
using System.Security.Cryptography;

namespace WaterLevellingAlgorithm.Alg
{
    internal class Model
    {
        public Cistern[] Cisterns { get; set; }
        public int CisternsCount => Cisterns.Length;
        public uint InjectedWaterVolume { get; set; }

        public void Read(TextReader tr)
        {
            int numberOfCisterns = Int32.Parse(tr.ReadLine()?? throw new InvalidOperationException());
            if (numberOfCisterns >= 1 && numberOfCisterns <= 50000)
            {
                Cisterns = new Cistern[numberOfCisterns];
                for (int i = 0; i < numberOfCisterns; i++)                              //Repeating the process of making Cistern
                {
                    string[] parameters = tr.ReadLine()?.Split(' ');                //Formatting line of text into array of text-arguments
                    uint[] parnum = new uint[parameters.Length - 1];
                    for (int j = 0; j < parameters.Length - 1; j++)
                    {
                        parnum[j] = UInt32.Parse(parameters[j + 1]);                      //Conversion from string to number
                        if (parnum[j] > 1000000)
                            throw new Exception("Cistern dimension error");
                    }

                    switch (parameters[0])                                              //Creating instance of Cistern from collected parameters
                    {
                        case "cub":
                            Cisterns[i] = new CuboidCistern(parnum[0], parnum[1], parnum[2], parnum[3]);
                            break;
                        case "cyl":
                            Cisterns[i] = new CylinderCistern(parnum[0], parnum[1], parnum[2]);
                            break;
                        case "con":
                            Cisterns[i] = new ConeCistern(parnum[0], parnum[1], parnum[2]);
                            break;
                        case "sph":
                            Cisterns[i] = new SphereCistern(parnum[0], parnum[1]);
                            break;
                    }
                }

                uint water = UInt32.Parse(tr.ReadLine() ?? throw new InvalidOperationException());                          //Collecting injected water value
                if (water < 1 || water > 2000000000)
                    throw new Exception("Injected water value not in <1;10^9> range!");
                else InjectedWaterVolume = water;
            }
        }

        public void ReadFromString(string line)
        {
            string[] pars = line.Split(' ');
            int numberOfCisterns = int.Parse(pars[0]);
            Cisterns = new Cistern[numberOfCisterns];
            int index = 1;
            for (int i = 0; i < numberOfCisterns; i++)
            {
                switch (pars[index]) //Creating instance of Cistern from collected parameters
                {
                    case "cub":
                        Cisterns[i] = new CuboidCistern(uint.Parse(pars[index + 1]), uint.Parse(pars[index + 2]),
                            uint.Parse(pars[index + 3]), uint.Parse(pars[index + 4]));
                        index += 5;
                        break;
                    case "cyl":
                        Cisterns[i] = new CylinderCistern(uint.Parse(pars[index + 1]), uint.Parse(pars[index + 2]), uint.Parse(pars[index + 3]));
                        index += 4;
                        break;
                    case "con":
                        Cisterns[i] = new ConeCistern(uint.Parse(pars[index + 1]), uint.Parse(pars[index + 2]), uint.Parse(pars[index + 3]));
                        index += 4;
                        break;
                    case "sph":
                        Cisterns[i] = new SphereCistern(uint.Parse(pars[index + 1]), uint.Parse(pars[index + 2]));
                        index += 3;
                        break;
                }
            }

            InjectedWaterVolume = uint.Parse(pars[pars.Length - 1]);
        }

        public bool IsOverflowed()
        {
            return ModelCapacity() < InjectedWaterVolume;
        }

        public int Roof()
        {
            int roof = Cisterns[0].Ceiling();
            foreach (Cistern c in Cisterns)
            {
                if (c.Ceiling() > roof)
                    roof = c.Ceiling();
            }
            return roof;
        }

        public double ModelCapacity()
        {
            double modelCapacity = 0;
            for (int i = 0; i < CisternsCount; i++)
                modelCapacity += Cisterns[i].Volume();
            return modelCapacity;
        }

        public double ComputeWaterLevel()
        {
            if (IsOverflowed())
                return -1;
            
            double div = 2;
            double max = Roof();
            double level = max / div;
            double tempVolume = 0;
            double difference = 0;
            foreach (Cistern cis in Cisterns)
                tempVolume += cis.CalculateVolumeFromLevel(level);

            do
            {
                div *= 2;
                if (tempVolume >= InjectedWaterVolume)
                    level -= max / div;
                else
                    level += max / div;

                tempVolume = 0;
                foreach (Cistern cis in Cisterns)
                    tempVolume += cis.CalculateVolumeFromLevel(level);
                difference = Math.Abs(InjectedWaterVolume - tempVolume);

            } while (difference > 0.01 || difference == 0 );
            return level;
        }
    }

}