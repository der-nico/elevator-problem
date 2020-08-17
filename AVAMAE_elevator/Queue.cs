using System;
using System.Collections.Generic;

namespace AVAMAE_elevator
{
    public class Queue
    {
        static int maxSpace = 8;
        public List<Command> commands;
        public Queue()
        {
            commands = new List<Command>();
        }
        public List<int> GetFloorsToGo(int time, int floor)
        {
            List <Command> commandsToGo = new List<Command>();
            List<int> floorsToGo = new List<int>();

            bool highestPriorityDirectionUp = true;
            double highestPriority = -1;

            Command closestUp = commands[0];
            Command closestDown = commands[0];
            int deltaUp = 99;
            int deltaDown = 99;

            foreach (var command in commands)
            {
               
                double priotrity = command.GetPriority(time);
                bool directionUp = command.GetDirectionUp(floor);
                int deltaFloor = Math.Abs(command.GetTaskFloor() - floor);
               
                commandsToGo.Add(command);
                floorsToGo.Add(command.GetTaskFloor());

                if (HasSpace(floor, command.floorFrom) || command.PickedUp)
                {
                    if (priotrity > highestPriority)
                    {
                        highestPriority = priotrity;
                        highestPriorityDirectionUp = directionUp;
                    }
                }
            }
            return floorsToGo;
        }
        public Command GetNextTask(int time, int floor)
        {
            bool highestPriorityDirectionUp = true;
            double highestPriority = -1;

            Command closestUp = commands[0];
            Command closestDown = commands[0];
            int deltaUp = 99;
            int deltaDown = 99;

            foreach (var command in commands)
            {
                double priotrity = command.GetPriority(time);
                if (command.id == 7999 || command.id == 49999 || command.id==699999)
                {
                    Console.WriteLine(command.id + " time="+time+ " " + priotrity);
                }
                //Console.Write(command.id + "here ");
                if (commands.Count > 500)
                {
                    Console.WriteLine("Task " + command.id + " priority=" + priotrity);
                }
                bool directionUp = command.GetDirectionUp(floor);
                int deltaFloor = Math.Abs(command.GetTaskFloor() - floor);

                if (!HasSpace(floor, command.floorFrom) && !command.PickedUp)
                {
                    priotrity = 0;
                }
                else
                {
                    if (directionUp && deltaUp > deltaFloor)
                    {
                        closestUp = command;
                        deltaUp = deltaFloor;
                    }
                    else if (!directionUp && deltaDown > deltaFloor)
                    {
                        closestDown = command;
                        deltaDown = deltaFloor;
                    }
                    if (priotrity > highestPriority)
                    {
                        highestPriority = priotrity;
                        highestPriorityDirectionUp = directionUp;
                    }
                }
            }
            if (highestPriorityDirectionUp)
            {
                return closestUp;
            }
            else
            {
                return closestDown;
            }
        }
        public bool IsEmpty()
        {

            return commands.Count ==0;
        }
        public void NextStep(int time, int floor, bool doSimulation=false)
        {
            for (int i = commands.Count - 1; i >= 0; i--)
            {
                Command command = commands[i];
                if (command.PickedUp)
                {
                    if (floor == command.floorTo)
                    {
                        /*if (!doSimulation)
                        {
                            Console.WriteLine("Finished task for " + command.id + " in " + (time - command.timeStart) + "s (" + (command.timePickUp - command.timeStart) + "s to pick up, minimal time=" + command.MinimalExecutionTime() + ")");
                        }*/
                        commands.Remove(command);
                    }
                }
            }
            for (int i = commands.Count - 1; i >= 0; i--)
            {
                Command command = commands[i];

                if (!command.PickedUp && HasSpace(floor, command.floorFrom))
                {
                    if (floor == command.floorFrom)
                    {
                        if (doSimulation)
                        {
                            commands.Remove(command);
                        }
                        else
                        {
                            //Console.WriteLine("Picked up " + command.id + " at floor " + command.floorFrom + " at " + time);
                            command.PickedUp = true;
                            command.timePickUp = time;
                        }
                    }
                }
            }
        }

        public bool HasSpace(int currentFloor, int floorTo)
        {
            int people = 0;
            foreach (var command in commands)
            {
                if (command.PickedUp)
                {
                    if (true)//((command.floorFrom < currentFloor) == (command.floorFrom < floorTo))
                    {
                        people += 1;
                    }
                }
            }
            if (people > 8)
            {
                Console.WriteLine("Space=" + people + " of " + maxSpace);
            }
            return people < maxSpace;
        }
        public void AddCommand(Command Command)
        {
            commands.Add(Command);
        }

    }
}
