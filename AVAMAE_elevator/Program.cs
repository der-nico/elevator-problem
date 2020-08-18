using System;
using System.Collections.Generic;



namespace AVAMAE_elevator
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            // Read the CSV data file
            ElevatorInputs inputData = new ElevatorInputs("../../../data.csv");
            ElevatorOutputs outputData = new ElevatorOutputs();

            //Initialise basics
            int time = 0;
            var building = new Building();
            var elevator = building.firstElevator;

            // Repeat until no more task are in the input Data and the elevator queue
            while (!elevator.queue.IsEmpty() || !inputData.isEmpty())
            {
                
                if (elevator.ArrivedAtFloor(time))
                {
                    elevator.Update(time);
                    building.PrintInfo(time, outputData);

                } while (inputData.RequestingNewTask(time))
                {
                    Command nextCommand = new Command(inputData.ApplyNextCommand(), elevator.GetFloor());

                    //Console.WriteLine("Task id: " + nextCommand.id + " requested at " + time);
                    building.AddTask(nextCommand, time);
                    //building.PrintInfo(time);

                }

                time += 1;
            }
            outputData.SaveData("../../../data_new.csv");
        }
        
    }
}