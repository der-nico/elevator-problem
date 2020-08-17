using System;
using System.IO;

using System.Collections.Generic;



namespace AVAMAE_elevator
{
    class Program
    {
        static List<CSVinput> ReadData(string filename)
        {
            List<CSVinput> inputData = new List<CSVinput>();

            using (var reader = new StreamReader(filename))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    CSVinput input = new CSVinput(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]));
                    inputData.Add(input);
                }
            }
            return inputData;
        }

        static void Main(string[] args)
        {
            // Read the CSV data file
            List<CSVinput> inputData = new List<CSVinput>();
            inputData = ReadData("../../../../data.csv");
           
            //Initialise basics
            int time = 0;
            var building = new Building();
            var elevator = building.firstElevator;
            int timeNextRequest = inputData[0].timeStart;
            int timeNextFinishedTask = -1;
            // Repeat until no more task are in the input Data and the elevator queue
            while (!elevator.queue.IsEmpty() || inputData.Count > 0)
            {

                // The stat of the elvator needs to be updated either when
                // A task finsihes or a ned task is requested
                if (time == timeNextFinishedTask || time == timeNextRequest)
                {
                    while (time == timeNextRequest)
                    {
                        //Console.WriteLine("Task id: " + inputData[0].id + " requested at " + time);

                        building.AddTask(new Command(inputData[0], elevator.GetFloor()));
                        inputData.RemoveAt(0);
                        if (inputData.Count > 0)
                        {
                            timeNextRequest = inputData[0].timeStart;
                        }
                        else
                        {
                            // All tasks have been added to the queue
                            timeNextRequest = -1;
                        }
                    }
                    elevator.Update(time);

                    if (time == timeNextFinishedTask)
                    {
                        building.PrintInfo(time);
                    }
                    timeNextFinishedTask = elevator.GetNextTaskEndTime(time);
                    //Console.("timeNextFinishedtask finished at: " + timeNextFinishedTask);
                }

                time += 1;
            }
        }
        
    }
}