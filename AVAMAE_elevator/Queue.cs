using System;
using System.Collections.Generic;
using System.Linq;

namespace AVAMAE_elevator
{
    public class Queue
    {
        const int MaxSpace = 8;

        public List<Command> Commands { get; set; } = new List<Command>();

        public Command GetNextTask(int time, int floor, int directionBefore)
        {
            int highestPriorityDirection = 0;
            double highestPriority = -1;

            Command closestUp = Commands[0];
            Command closestDown = Commands[0];
            int minDeltaUp = 99;
            int minDeltaDown = 99;

            foreach (var command in Commands)
            {
                double priority = command.GetPriority(time, this, floor, command.FloorFrom);
                if (priority > 0)
                {
                    int direction = command.GetDirection(floor + directionBefore);
                    int deltaFloor = Math.Abs(command.TaskFloor - floor);

                    if (deltaFloor == 0)
                    {
                        //If the elevator is already on this floor we dont have to continue this loop
                        return command;
                    }
                    if (direction > 0 && minDeltaUp > deltaFloor)
                    {
                        closestUp = command;
                        minDeltaUp = deltaFloor;
                    }
                    else if (direction < 0 && minDeltaDown > deltaFloor)
                    {
                        closestDown = command;
                        minDeltaDown = deltaFloor;
                    }
                    if (priority > highestPriority)
                    {
                        highestPriority = priority;
                        highestPriorityDirection = direction;
                    }
                }
            }
            return highestPriorityDirection > 0 ? closestUp : closestDown;
        }
        public bool IsEmpty() => Commands.Count == 0;

        public void NextStep(int time, int floor, bool doSimulation = false)
        {
            // First let all people leave (in reverse order)
            for (int i = Commands.Count - 1; i >= 0; i--)
            {
                Command command = Commands[i];
                if (command.PickedUp)
                {
                    if (floor == command.TaskFloor)
                    {
                        if (!doSimulation)
                        {
                            Console.WriteLine($"Finished task for {command.Id} in {time - command.TimeStart}s ({command.TimePickUp - command.TimeStart}s to pick up, minimal time={ command.MinimalExecutionTime})");
                        }
                        Commands.Remove(command);
                    }
                }
            }
            List<int> PickUp = new List<int>();
            for (int i = 0; i < Commands.Count; i++)
            {
                Command command = Commands[i];

                if (!command.PickedUp && HasSpace(floor, command))
                {         
                    if (floor == command.TaskFloor)
                    {
                        if (doSimulation)
                        {
                            PickUp.Add(i);
                        }
                        else
                        {
                            //Console.WriteLine("Picked up " + command.id + " at floor " + command.floorFrom + " at " + time);
                            command.PickedUp = true;
                            command.TimePickUp = time;
                        }
                    }
                }
            }
            if (PickUp.Count > 0)
            {
                for (int i = PickUp.Count - 1; i >= 0; i--)
                {
                    Commands.RemoveAt(PickUp[i]);
                }
            }

        }
        public bool IsInBetween(int staritngFloor, int endingFloor, Command command)
        {
            return ((staritngFloor < command.TaskFloor) == (endingFloor < command.TaskFloor));
        }

        public bool HasSpace(int currentFloor, Command command)
        {
            int peopleInElevator = 0;
            foreach (var otherCommand in Commands.Where((Command c) => c != command))
            {
                // Only count people that will still be in the elvator
                // Add all people that will get in in the meantitime
                if (IsInBetween(currentFloor, otherCommand.FloorFrom, command) != otherCommand.PickedUp)
                {
                    peopleInElevator++;
                }
            }
            
            return peopleInElevator < MaxSpace;
        }
        public void AddCommand(Command Command) => Commands.Add(Command);

    }
}
