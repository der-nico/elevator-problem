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

        public void PrintInfo(int time, ElevatorOutputs dataOutput)
        {
            // Function pritning the current state of the elvator and some additional info
            CSVOutput data = new CSVOutput();
            data.time = time;
            data.currentFloor = firstElevator.GetFloor();
            foreach (var command in firstElevator.queue.commands)
            {
                if (command.PickedUp)
                {
                    data.peopleInElevator.Add(command.id);
                }
            }
            Console.Write("State: t=" + time);
            Console.Write(", people id's=");
            foreach (var id in data.peopleInElevator)
            {
                Console.Write(id + ", ");
            }
            Console.Write(" currentfloor=" + firstElevator.GetFloor());

            if (!firstElevator.queue.IsEmpty())
            {
                List<int> floors = GetOrderedListOfFloors(time, firstElevator);
                foreach (var floorToGo in floors)
                {
                    data.sortedQueue.Add(floorToGo);
                }
            }
            Console.Write(", floorstogo=");
            foreach (var floorToGo in data.sortedQueue)
            {
                Console.Write(floorToGo + ", ");
            }
            Console.WriteLine();
            dataOutput.addData(data);
        }
        
    

        public void AddTask(Command command, int time)
        {
            // Adding the task to the elevator queue
            // In more complex building this could decide which elevator get assigned the task
            firstElevator.AddTask(command, time);
        }
        public List<int> GetOrderedListOfFloors(int time, Elevator elevator)
        {
            // Simulate the behaviour of the elevaotr given the current open tasks
            // return the list of sorted floors the elevator is aiming at
            Elevator simulatedElevator = new Elevator(elevator, time);
            
            List<int> floors = new List<int>();
            
            while (!simulatedElevator.queue.IsEmpty())
            {
                bool doSimulation = true;
                bool hasMoved = false;

                if (simulatedElevator.ArrivedAtFloor(time))
                {
                    simulatedElevator.Update(time, doSimulation);
                    floors.Add(simulatedElevator.GetFloor());
                }
                time += 1;
            }
            return floors;
        
        }
    }
}
