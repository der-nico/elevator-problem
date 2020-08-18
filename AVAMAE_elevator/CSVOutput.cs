using System;
using System.Collections.Generic;

namespace AVAMAE_elevator
{
    public class CSVOutput
    {
        public int time;
        public List<int> peopleInElevator = new List<int>();
        public int currentFloor;
        public List<int> sortedQueue = new List<int>();

        public CSVOutput()
        {
            this.time = 0;
            this.currentFloor = 0;
        }
        public CSVOutput(int time, List<int> peopleInElevator, int currentFloor, List<int> sortedQueue)
        {
            this.time = time;
            this.peopleInElevator = peopleInElevator;
            this.currentFloor = currentFloor;
            this.sortedQueue = sortedQueue;
        }
    }
}
