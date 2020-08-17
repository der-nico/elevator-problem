using System;

using System.Collections.Generic;

namespace AVAMAE_elevator
{
    public class Building
    {
        public Elevator firstElevator;

        public Building()
        {
            firstElevator = new Elevator();
            
        }

        public Elevator GetElevator()
        {
            return this.firstElevator;
        }

        public void PrintInfo(int time)
        {
            // Function pritning the current state of the elvator and some additional info
            if (!firstElevator.queue.IsEmpty())
            {
                List<int> people = new List<int>();
                foreach (var command in firstElevator.queue.commands)
                {
                    if (command.PickedUp)
                    {
                        people.Add(command.id);
                    }
                }
                List<int> floors = GetOrderedListOfFloors(time, firstElevator);

                Console.Write("State: t=" + time);
                Console.Write(", people id's=");
                foreach (var id in people)
                {
                    Console.Write(id + ", ");
                }
                Console.Write(" currentfloor=" + firstElevator.GetFloor());
                Console.Write(", floorstogo=");

                foreach (var floorToGo in floors)
                {
                    Console.Write(floorToGo + ", ");
                }
                Console.WriteLine();
            }
            else
            {
                Console.Write("State: t=" + time);
                Console.Write(", people id's=,");
                
                Console.Write(" currentfloor=" + firstElevator.GetFloor());
                Console.Write(", floorstogo=");

                Console.WriteLine();

            }
        }

        public void AddTask(Command command)
        {
            // Adding the task to the elevator queue
            // In more complex building this could decide which elevator get assigned the task
            firstElevator.AddTask(command);
        }
        public List<int> GetOrderedListOfFloors(int time, Elevator elevator)
        {
            // Simulate the behaviour of the elevaotr given the current open task
            // return the list of sorted floors the elevator is aiming at
            Elevator simulatedElevator = new Elevator(elevator);
            int timeNextFinishedTask = simulatedElevator.GetNextTaskEndTime(time);

            List<int> floors = new List<int>();
            while (!simulatedElevator.queue.IsEmpty())
            {

                bool doSimulation = true;
                simulatedElevator.Update(time, doSimulation);
                bool hasMoved = false;
                if (time == timeNextFinishedTask)
                {
                    if (elevator.GetFloor() != simulatedElevator.GetFloor() || hasMoved)
                    {
                        floors.Add(simulatedElevator.GetFloor());
                        hasMoved = true;
                    }
                }
                timeNextFinishedTask = simulatedElevator.GetNextTaskEndTime(time);
                time += 1;
            }
            return floors;
        
        }
    }
}
