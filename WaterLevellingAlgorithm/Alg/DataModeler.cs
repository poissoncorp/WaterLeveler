using System;
using System.Globalization;
using System.IO;

namespace WaterLevellingAlgorithm.Alg
{
    class DataModeler
    {
        private Model _currentModel;
        private string[] _output;
        public void ReadDataSets(string filename)
        {
            TextReader tr = File.OpenText(filename);
            int dataSets = Int32.Parse(tr.ReadLine() ?? throw new InvalidOperationException());
            if (dataSets <= 30 && dataSets >= 1)
            {
                _output = new string[dataSets];
                for (int i = 0; i < dataSets; i++)
                {
                    _currentModel = new Model();
                    _currentModel.Read(tr);
                    string x = _currentModel.ComputeWaterLevel().ToString(CultureInfo.CurrentCulture);
                    if (x == "-1")
                        _output[i] = "OVERFLOW";
                    else
                        _output[i] = x;
                }
            }
            else
                throw new Exception("Arg error, not in <1;30> range");
        }

        public Cistern[] GetCurrentModelCisterns() // Created in order to make it easier for Unity to read size of preloaded cisterns 
        {                                          // w/o need to analyze the input string one more time at Unity sided code
            return _currentModel.Cisterns;
        }


        public double ModelAssignAndLevellingFromString(string line)
        {
            _currentModel = new Model();
            _currentModel.ReadFromString(line);
            return _currentModel.ComputeWaterLevel();
        }

        public void PrintWaterLevels()
        {
            foreach (string o in _output)
                Console.WriteLine(o != "OVERFLOW"
                    ? Math.Round(Double.Parse(o), 2).ToString(CultureInfo.CurrentCulture)
                    : o);
        }
    }

}