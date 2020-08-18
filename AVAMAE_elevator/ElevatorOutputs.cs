using System;
using System.IO;
using System.Collections.Generic;

namespace AVAMAE_elevator
{
    public class ElevatorOutputs
    {
        List<CSVOutput> outputData = new List<CSVOutput>();

        public ElevatorOutputs()
        {

        }
        public void addData(CSVOutput data)
        {
            outputData.Add(data);
        }
        public void SaveData(string filename)
        {
            using (var w = new StreamWriter(filename))
            {
                w.WriteLine("Time, People in elevator, Elevator floor, Sorted queue)");
                w.Flush();
                foreach (CSVOutput data in outputData)
                {
      
                    var line = string.Format("{0},", data.time);
                    foreach (var people in data.peopleInElevator)
                    {
                        line += string.Format("{0};", people);
                    }
                    line += string.Format(",{0},", data.currentFloor);
                    foreach (var floor in data.sortedQueue)
                    {
                        line += string.Format("{0};", floor);
                    }
                    w.WriteLine(line);
                    w.Flush();
                }
            }
        }
        
    }
}
