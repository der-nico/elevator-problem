using System;
namespace AVAMAE_elevator
{
    public class Command
    {
        public int id;
        public int floorElevatorStart;
        public int floorFrom;
        public int floorTo;
        public int timeStart;
        public int timePickUp;
        public bool PickedUp;
        public Command()
        {
            this.id = -1;
            this.floorFrom = -1;
            this.floorTo = -1;
            this.timeStart = -1;
            this.timePickUp = -1;
            this.floorElevatorStart = -1;
            this.PickedUp = false;
        }
        public Command(
            int id, int floorFrom, int floorTo, int timeStart,
            int floorElevatorStart)
        {
            this.id = id;
            this.floorFrom = floorFrom;
            this.floorTo = floorTo;
            this.timeStart = timeStart;
            this.timePickUp = -1;
            this.floorElevatorStart = floorElevatorStart;
            this.PickedUp = false;
        }
        public Command(
           CSVinput input,
           int floorElevatorStart)
        {
            this.id = input.id;
            this.floorFrom = input.floorFrom;
            this.floorTo = input.floorTo;
            this.timeStart = input.timeStart;
            this.timePickUp = -1;
            this.floorElevatorStart = floorElevatorStart;
            this.PickedUp = false;
        }
        public int GetTaskFloor()
        {
            if (PickedUp)
            {
                return floorTo;
            }
            else
            {
                return floorFrom;

            }
        }
        public int GetDeltaFloor()
        {
            int deltaFloor = -1;
            if (PickedUp)
            {
                deltaFloor = Math.Abs(floorFrom - floorTo);
            }
            else
            {
                deltaFloor = Math.Abs(floorElevatorStart - floorFrom);

            }
            if (deltaFloor == 0)
            {
                // Remove diviosn by 0 
                // In case the elvator is on this floor
                // the task will be performed if possible and the priority is not necessary
                deltaFloor = 1;
            }
            return deltaFloor;
        }
        public int GetDirection(int floor)
        {
            if (PickedUp)
            {
                return (floor < floorTo) ? 1 : -1;
            }
            else
            {
                return (floor < floorFrom) ? 1 : -1;
            }
        }

        public double GetPriority(int t, Queue queue, int currentFloor, int floorFrom)
        {
            if (!queue.HasSpace(currentFloor, this))
            {
                return 0.0;
            }
            double priority = 1.0;
            int deltaFloor = GetDeltaFloor();
            priority = priority / (deltaFloor);
            int timeNext = GetNextTime();
            priority = priority * Math.Exp((t - timeNext) / 10.0);
              return priority;
        }
        public int GetNextTime()
        {
            if (PickedUp)
            {
                return timePickUp;
            }
            else
            {
                return timeStart;

            }

        }

        public int GetExecutionTime(int floor)
        {
            int time = 0;
            if (PickedUp)
            {
                time = 10 * Math.Abs(floor - floorTo);
            }
            else
            {
                time = 10 * Math.Abs(floor - floorFrom);
            }

            return time;
        }
        public int MinimalExecutionTime()
        {
            int DeltaFloorFirst = Math.Abs(floorElevatorStart - floorFrom);

            int DeltaFloorSecond = Math.Abs(floorFrom - floorTo);
            return 10 * (DeltaFloorFirst + DeltaFloorSecond);
        }
    }
}
