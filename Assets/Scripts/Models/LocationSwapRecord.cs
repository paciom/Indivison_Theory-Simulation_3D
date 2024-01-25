using UnityEngine;

namespace Models
{
    public class LocationSwapRecord
    {
        public Vector3 Start;
        public Vector3 End;
        public int Time;

        public LocationSwapRecord(Vector3 start, Vector3 end, int time)
        {
            Start = start;
            End = end;
            Time = time;
        }
    }
}
