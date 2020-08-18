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
        
        public Command GetNextTask(int time, int floor, int directionBefore)
        {
            int highestPriorityDirection = 0;
            double highestPriority = -1;

            Command closestUp = commands[0];
            Command closestDown = commands[0];
            int deltaUp = 99;
            int deltaDown = 99;

            foreach (var command in commands)
            {
                double priority = command.GetPriority(time, this, floor, command.floorFrom);


                if (priority > 0)
                {

                    int direction = command.GetDirection(floor+directionBefore);
                    int deltaFloor = Math.Abs(command.GetTaskFloor() - floor);

                    if (deltaFloor == 0)
                    {
                        return command;
                    }
                    
                    if (direction > 0 && deltaUp > deltaFloor)
                    {
                        closestUp = command;
                        deltaUp = deltaFloor;
                    }
                    else if (direction < 0 && deltaDown > deltaFloor)
                    {
                        closestDown = command;
                        deltaDown = deltaFloor;
                    }
                    if (priority > highestPriority)
                    {
                        highestPriority = priority;
                        highestPriorityDirection = direction;
                    }
                }
            }
            if (highestPriorityDirection>0)
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

            return commands.Count == 0;
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
                        if (!doSimulation)
                        {
                            Console.WriteLine("Finished task for " + command.id + " in " + (time - command.timeStart) + "s (" + (command.timePickUp - command.timeStart) + "s to pick up, minimal time=" + command.MinimalExecutionTime() + ")");
                        }
                        commands.Remove(command);
                    }
                }
            }
            List<int> PickUp = new List<int>();
            for (int i = 0; i < commands.Count; i++)
            {
                Command command = commands[i];

                if (!command.PickedUp && HasSpace(floor, command))
                {         
                    if (floor == command.floorFrom)
                    {
                        if (doSimulation)
                        {
                            //Console.WriteLine("Picked up " + command.id + " at floor " + command.floorFrom + " at " + time);

                            PickUp.Add(i);
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
            if (PickUp.Count > 0) {
                for (int i = PickUp.Count - 1; i >= 0; i--)
                {
                    //Console.Write(PickUp[i] + "id " +commands[PickUp[i]].id+" "+commands[PickUp[i]].PickedUp+",");
                    commands.RemoveAt(PickUp[i]);
                }
                //Console.WriteLine();
            }

        }

        public bool HasSpace(int currentFloor, Command command)
        {
            int peopleInElevator = 0;
            bool checkSelf = false;
            foreach (var othercommand in commands)
            {
                if (command == othercommand)
                {
                    checkSelf = true;
                }
                {

                    if (othercommand.PickedUp)
                    {
                        // Only count people that will still be in the elvator
                        if ((othercommand.floorTo < currentFloor) == (othercommand.floorTo < command.floorFrom ))
                        {
                            peopleInElevator += 1;
                        }
                        
                    }
                    else
                    {
                        // Add all people that will get in in the meantitime
                        if ((othercommand.floorFrom < currentFloor) != (othercommand.floorFrom < command.floorFrom))
                        {
                            peopleInElevator += 1;
                        }

                    }
                }
            }
            return peopleInElevator < maxSpace;
        }
        public void AddCommand(Command Command)
        {
            commands.Add(Command);
        }

    }
}
