using UnityEngine;

namespace Collisions
{
    public class CollisionRecord
    {
        public int DotId { get; }
        public int Time { get; }
        public Vector3 Location { get; }

        public CollisionRecord(int dotId, int time, Vector3 location)
        {
            DotId = dotId;
            Time = time;
            Location = location;
        }
    }
}
