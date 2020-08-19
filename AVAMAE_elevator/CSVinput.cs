using System;
namespace AVAMAE_elevator
{
    public class CSVinput
    {
        public int Id { get; set; }
        public int FloorTo { get; set; }
        public int TimeStart { get; set; }
        public int FloorFrom { get; set; }

        public CSVinput()
        {
            Id = -1;
            FloorFrom = -1;
            FloorTo = -1;
            TimeStart = -1;
        }
        public CSVinput(int id, int floorFrom, int floorTo, int timeStart)
        {
            Id = id;
            FloorFrom = floorFrom;
            FloorTo = floorTo;
            TimeStart = timeStart;
        }
    } 
}
