using System;

using System.Collections.Generic;
using System.Linq;

namespace AVAMAE_elevator
{
    public class Building
    {
        public Elevator FirstElevator { get; set; } = new Elevator();


        public Elevator GetElevator() => FirstElevator;

        public void StoreAndPrintInfo(int time, ElevatorOutputs dataOutput)
        {
            // Function pritning the current state of the elvator and some additional info
            // and writingthe elevator states to the ElevatorOutputs class
            CSVOutput data = new CSVOutput
            {
                Time = time,
                CurrentFloor = FirstElevator.GetFloor()
            };
            foreach (var command in FirstElevator.queue.Commands.Where((Command c) => c.PickedUp))
            {
                data.PeopleInElevator.Add(command.Id);
            }
            Console.Write($"State: t={time}, ");
            Console.Write($"people id's={string.Join(", ",data.PeopleInElevator)}, ");
            Console.Write($" currentfloor={FirstElevator.GetFloor()}");

            if (!FirstElevator.queue.IsEmpty())
            {
                List<int> floors = GetOrderedListOfFloors(time, FirstElevator);
                foreach (var floorToGo in floors)
                {
                    data.SortedQueue.Add(floorToGo);
                }
            }
            Console.WriteLine($", floorstogo={string.Join(", ", data.SortedQueue)}");
            dataOutput.AddData(data);
        }

        public void AddTask(Command command, int time) => FirstElevator.AddTask(command, time);

        public List<int> GetOrderedListOfFloors(int time, Elevator elevator)
        {
            // Simulate the behaviour of the elevaotr given the current open tasks
            // return the list of sorted floors the elevator is aiming at
            //Copy the elvator using the same instance of the queue
            Elevator simulatedElevator = elevator.Clone();
            simulatedElevator.UpdateState(time);

            List<int> floors = new List<int>();
            
            while (!simulatedElevator.queue.IsEmpty())
            {
                bool doSimulation = true;
                if (simulatedElevator.ArrivedAtFloor(time))
                {
                    simulatedElevator.UpdateState(time, doSimulation);
                    floors.Add(simulatedElevator.GetFloor());
                }
                time++;

            }
            return floors;
        
        }
    }
}
