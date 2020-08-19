using System;
namespace AVAMAE_elevator
{
    public class Command
    {
        public int FloorFrom { get; set; }
        public int Id { get; set; }
        public int FloorElevatorStart { get; set; }
        public int FloorTo { get; set; }
        public int TimeStart { get; set; }
        public int TimePickUp { get; set; }
        public bool PickedUp { get; set; }

        public Command()
        {
            Id = -1;
            FloorFrom = -1;
            FloorTo = -1;
            TimeStart = -1;
            TimePickUp = -1;
            FloorElevatorStart = -1;
            PickedUp = false;
        }
        public Command(int id,
                       int floorFrom,
                       int floorTo,
                       int timeStart,
                       int floorElevatorStart)
        {
            Id = id;
            FloorFrom = floorFrom;
            FloorTo = floorTo;
            TimeStart = timeStart;
            TimePickUp = -1;
            FloorElevatorStart = floorElevatorStart;
            PickedUp = false;
        }
        public Command(CSVinput input,
                       int floorElevatorStart)
        {
            Id = input.Id;
            FloorFrom = input.FloorFrom;
            FloorTo = input.FloorTo;
            TimeStart = input.TimeStart;
            TimePickUp = -1;
            FloorElevatorStart = floorElevatorStart;
            PickedUp = false;
        }
        public int TaskFloor => PickedUp ? FloorTo : FloorFrom;

        public int DeltaFloor
        {
            get
            {
                int deltaFloor;
                if (PickedUp)
                {
                    deltaFloor = Math.Abs(FloorFrom - FloorTo);
                }
                else
                {
                    deltaFloor = Math.Abs(FloorElevatorStart - FloorFrom);

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
        }

        public int GetDirection(int floor) => (floor < TaskFloor) ? 1 : -1;

        public double GetPriority(int t,
                                  Queue queue,
                                  int currentFloor,
                                  int floorFrom)
        {
            if (!queue.HasSpace(currentFloor, this))
            {
                return 0.0;
            }
            double priority = 1.0;
            priority = priority / (DeltaFloor);
            priority *= Math.Exp((t - TaskTime) / 10.0);
            return priority;
        }

        public int TaskTime => PickedUp ? TimePickUp : TimeStart;

        public int GetExecutionTime(int floor)
        {
            return 10 * Math.Abs(floor - TaskFloor);
        }
        public int MinimalExecutionTime
        {
            get
            {
                int DeltaFloorFirst = Math.Abs(FloorElevatorStart - FloorFrom);

                int DeltaFloorSecond = Math.Abs(FloorFrom - FloorTo);
                return 10 * (DeltaFloorFirst + DeltaFloorSecond);
            }
        }
    }
}
