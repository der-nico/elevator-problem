using System;
using System.IO;
using System.Collections.Generic;

namespace AVAMAE_elevator
{
    public class ElevatorOutputs
    {
        public ElevatorOutputs()
        {

        }

        public List<CSVOutput> OutputData { get; set; } = new List<CSVOutput>();

        public void AddData(CSVOutput data) => OutputData.Add(data);
        public void SaveData(string filename)
        {
            using var w = new StreamWriter(filename);
            w.WriteLine("Time, People in elevator, Elevator floor, Sorted queue)");
            w.Flush();
            foreach (CSVOutput data in OutputData)
            {
                string line = $"{data.Time},";
                line += $"{string.Join(";", data.PeopleInElevator)},";
                line += $"{data.CurrentFloor},";
                line += $"{string.Join(";", data.SortedQueue)}";
                w.WriteLine(line);
                w.Flush();
            }
        }
        
    }
}
