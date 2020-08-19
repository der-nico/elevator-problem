using System;
using System.Collections.Generic;

namespace AVAMAE_elevator
{
    public class CSVOutput
    {
        public int Time { get; set; }
        public List<int> PeopleInElevator { get; set; } = new List<int>();
        public int CurrentFloor { get; set; }
        public List<int> SortedQueue { get; set; } = new List<int>();

        public CSVOutput()
        {
            Time = 0;
            CurrentFloor = 0;
        }
        public CSVOutput(int time, List<int> peopleInElevator, int currentFloor, List<int> sortedQueue)
        {
            Time = time;
            PeopleInElevator = peopleInElevator;
            CurrentFloor = currentFloor;
            SortedQueue = sortedQueue;
        }
    }
}
