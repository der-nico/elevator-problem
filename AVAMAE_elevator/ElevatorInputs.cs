using System;
using System.IO;
using System.Collections.Generic;
namespace AVAMAE_elevator
{
    public class ElevatorInputs
    {
        public int NextTaskTime { get; set; }
        public List<CSVinput> InputData { get; set; } = new List<CSVinput>();

        public ElevatorInputs()
        {
            NextTaskTime = -1;
        }

        public ElevatorInputs(string filename)
        {
            InputData = ReadData(filename);
            NextTaskTime = InputData[0].TimeStart;
        }

        public List<CSVinput> ReadData(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                reader.ReadLine();
                //for (int i = 0; i < 11; i++)
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    string[] values = line.Split(',');
                    CSVinput input = new CSVinput( int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]));
                    InputData.Add(input);
                }
            }
            return InputData;
        }
        public CSVinput ApplyNextCommand()
        {
            // Get the next command and remove it from inputlist
            //Here i assume the commands are always sorted
            CSVinput NextTask = GetNextCommand();
            InputData.RemoveAt(0);
            if (!IsEmpty)
            {
                NextTaskTime = InputData[0].TimeStart;
            }
            else
            {
                NextTaskTime = -1;
            }
            return NextTask;
        }
        public CSVinput GetNextCommand() => InputData[0];

        public bool IsEmpty => InputData.Count == 0;

        public bool RequestingNewTask(int time) => time == NextTaskTime;
    }
}
