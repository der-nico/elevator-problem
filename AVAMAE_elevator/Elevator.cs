using System;
using System.Collections.Generic;

namespace AVAMAE_elevator
{
    public class Elevator
    {
        int currentFloor = 0;
        int direction = 0;
        bool atFloor = true;
        int timeMoveStart = -1;
        int timeFinishedNextTask = -1;
        public Queue queue;
        static int timeForFloor = 10;

        public Elevator()
        {
            // Default constructor
            queue = new Queue();
            currentFloor = 0;
            direction = 0;
        }
        public Elevator(Elevator elevator, int time)
        {
            // Copy constructor
            currentFloor = elevator.currentFloor;
            direction = elevator.direction;
            atFloor = elevator.atFloor;
            timeMoveStart = elevator.timeMoveStart;
            queue = new Queue();
            foreach (var command in elevator.queue.commands)
            {
                queue.AddCommand(command);
            }

            timeFinishedNextTask = GetNextTaskEndTime(time);

        }

        public void Update(int time, bool doSimulation=false)
        {
            // Update the elevator position for the time time
            // If the elevator is simulated the task in pickup
            // state stop after picking up the person
            if (timeMoveStart <= time && timeMoveStart != -1)
            {
                int levelBefore = currentFloor;
                int deltaFloor = (time - timeMoveStart - 1 + timeForFloor) / (timeForFloor);
            
                timeMoveStart += timeForFloor * deltaFloor;
                if (direction > 0)
                {
                    currentFloor += deltaFloor;
                }
                else if (direction < 0)
                {
                    currentFloor -= deltaFloor;
                }
            
            }
            atFloor = (time - timeMoveStart) % timeForFloor == 0;

            if (atFloor)
            {
                direction = 0;
                timeMoveStart = -1;

                queue.NextStep(time, currentFloor, doSimulation);

            }
            ExecuteNextTask(time);
            timeFinishedNextTask = GetNextTaskEndTime(time);
        }

        public Command NextTask(int time)
        {
            // Get next task in queue

            return queue.GetNextTask(time, currentFloor, direction);
        }
        public void ExecuteNextTask(int time)
        {

            if (queue.IsEmpty())
            {
                direction = 0;
                return;
            }
            Command task = NextTask(time);
            if (timeMoveStart == -1)
            {
                timeMoveStart = time;
            }
            int floorTask = -1;
            if (task.PickedUp)
            {
                floorTask = task.floorTo;
            }
            else
            {
                floorTask = task.floorFrom;
            }

            if (floorTask > currentFloor)
            {
                direction = 1;
            }
            else if (floorTask< currentFloor)
            {
                direction = -1;

            }
            else
            {
                direction = 0;
            }
        }

        public int GetNextTaskExecutionTime(int time)
        {
            // Get the exectution time for the next Task
            if (queue.IsEmpty())
            {
                return 0;
            }
            Command task = NextTask(time);
            int timeToComplete = task.GetExecutionTime(currentFloor);

            timeToComplete += timeMoveStart - time;
            return timeToComplete;

        }

        public int GetNextTaskEndTime(int time)
        {
            // Get time at the end of the next task
            return GetNextTaskExecutionTime(time) + time;
        }

        public int GetFloor()
        { 
            return this.currentFloor;
        }
        public bool ArrivedAtFloor(int time)
        {
            return (timeFinishedNextTask==time);
        }
        public void AddTask(Command command, int time)
        {
            // Add task to the queue
            queue.AddCommand(command);
            Update(time);
           
        }
        /*public bool AddTask(Command command)
        {
            if (queue.HasSpace())
            {
                queue.AddCommand(command);
                return true;
            }
            else
            {
                return false;
            }

        }*/

    }
}
