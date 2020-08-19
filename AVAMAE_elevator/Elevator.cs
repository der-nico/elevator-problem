using System;
using System.Collections.Generic;

namespace AVAMAE_elevator
{
    public class Elevator
    {
        public Queue queue;
        static int timeForFloor = 10;

        public int CurrentFloor { get; set; } = 0;
        public int Direction { get; set; } = 0;
        public bool AtFloor { get; set; } = true;
        public int TimeMoveStart { get; set; } = 0;
        public int TimeFinishedNextTask { get; set; } = -1;

        public Elevator()
        {
            queue = new Queue();
            CurrentFloor = 0;
            Direction = 0;
        }
        public Elevator Clone()
        {
            return new Elevator(this);

        }
        public Elevator(Elevator elevator)
        {
            // Copy constructor
            CurrentFloor = elevator.CurrentFloor;
            Direction = elevator.Direction;
            AtFloor = elevator.AtFloor;
            TimeMoveStart = elevator.TimeMoveStart;
            queue = new Queue();
            foreach (var command in elevator.queue.Commands)
            {
                queue.AddCommand(command);
            }

        }

        public bool IsAtFloor(int time)
        {
            return (time - TimeMoveStart) % timeForFloor == 0;
        }
        public void UpdateState(int time, bool doSimulation = false)
        {
            // Update the elevator position for the time time
            // If the elevator is simulated the task in pickup
            // state stop after picking up the person
            if (TimeMoveStart <= time && TimeMoveStart != 0)
            {
                int levelBefore = CurrentFloor;
                int deltaFloor = (time - TimeMoveStart - 1 + timeForFloor) / timeForFloor;
            
                TimeMoveStart += timeForFloor * deltaFloor;

                CurrentFloor += Direction * deltaFloor;
            }
            
            if (IsAtFloor(time))
            {
                Direction = 0;
                TimeMoveStart = 0;

                queue.NextStep(time, CurrentFloor, doSimulation);

            }
            ExecuteNextTask(time);
            TimeFinishedNextTask = GetNextTaskEndTime(time);
        }

        public Command NextTask(int time) => queue.GetNextTask(time, CurrentFloor, Direction);

        public void ExecuteNextTask(int time)
        {

            if (queue.IsEmpty())
            {
                Direction = 0;
                return;
            }
            Command task = NextTask(time);
            if (TimeMoveStart == 0)
            {
                TimeMoveStart = time;
            }
            int floorTask =task.TaskFloor;

            if (floorTask > CurrentFloor)
            {
                Direction = 1;
            }
            else if (floorTask< CurrentFloor)
            {
                Direction = -1;

            }
            else
            {
                Direction = 0;
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
            int timeToComplete = task.GetExecutionTime(CurrentFloor);

            timeToComplete += TimeMoveStart - time;
            return timeToComplete;

        }

        public int GetNextTaskEndTime(int time)
        {
            // Get time at the end of the next task
            return GetNextTaskExecutionTime(time) + time;
        }

        public int GetFloor()
        { 
            return this.CurrentFloor;
        }
        public bool ArrivedAtFloor(int time)
        {
            return (TimeFinishedNextTask==time);
        }
        public void AddTask(Command command, int time)
        {
            // Add task to the queue
            queue.AddCommand(command);
            UpdateState(time);

           
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
