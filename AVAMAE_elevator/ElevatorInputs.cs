using System;
using System.IO;
using System.Collections.Generic;
namespace AVAMAE_elevator
{
    public class ElevatorInputs
    {

        List<CSVinput> inputData = new List<CSVinput>();
        int nextTaskTime;

        public ElevatorInputs()
        {
            nextTaskTime = -1;
        }
        public ElevatorInputs(string filename)
        {
            inputData = ReadData(filename);
            nextTaskTime = inputData[0].timeStart;
        }

        List<CSVinput> ReadData(string filename)
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
                    inputData.Add(input);
                }
            }
            return inputData;
        }
        public CSVinput ApplyNextCommand()
        {
            // Get the next command and remove it from inputlist
            CSVinput NextTask = GetNextCommand();
            inputData.RemoveAt(0);
            if (!isEmpty())
            {
                nextTaskTime = inputData[0].timeStart;
            }
            else
            {
                nextTaskTime = -1;
            }
            return NextTask;
        }
        public CSVinput GetNextCommand()
        {
            return inputData[0];
        }

        public bool isEmpty()
        {
            return inputData.Count == 0;
        }

        public bool RequestingNewTask(int time)
        {
            return (time == nextTaskTime);
        }
    }
}
