using System;
namespace AVAMAE_elevator
{
    public class CSVinput
    {
        public int id;
        public int floorFrom;
        public int floorTo;
        public int timeStart;

        public CSVinput()
        {
            this.id = -1;
            this.floorFrom = -1;
            this.floorTo = -1;
            this.timeStart = -1;
        }
        public CSVinput(int id, int floorFrom, int floorTo, int timeStart)
        {
            this.id = id;
            this.floorFrom = floorFrom;
            this.floorTo = floorTo;
            this.timeStart = timeStart;
        }
    }


   
}
