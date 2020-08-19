using System;
using System.Collections.Generic;



namespace AVAMAE_elevator
{
    class Program
    {


        static void Main(string[] args)
        {

            // Read the CSV data file
            ElevatorInputs inputData = new ElevatorInputs("data.csv");
            ElevatorOutputs outputData = new ElevatorOutputs();

            //Initialise basics
            int time = 0;
            Building building = new Building();
            Elevator elevator = building.FirstElevator;

            // Repeat until no more task are in the input Data and the elevator queue
            while (!elevator.queue.IsEmpty() || !inputData.IsEmpty)
            {
                
                if (elevator.ArrivedAtFloor(time))
                {
                    elevator.UpdateState(time);
                    building.StoreAndPrintInfo(time, outputData);
                }
                while (inputData.RequestingNewTask(time))
                {
                    Command nextCommand = new Command(inputData.ApplyNextCommand(), elevator.GetFloor());
                    //Console.WriteLine("Task id: " + nextCommand.id + " requested at " + time);
                    building.AddTask(nextCommand, time);
                }

                time++;
            }
            outputData.SaveData("data_output.csv");
        }
        
    }
}