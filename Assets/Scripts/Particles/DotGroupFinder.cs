using System;
using System.Collections.Generic;
using Collisions;
using Models;
using Zenject;

namespace Particles
{
    public class DotGroupFinder
    {
        [Inject]
        public DotGroupFinder()
        {
            
        }
        public List<Dot> FindGroup(Dot dot, int swapHistoryLength)
        {
            List<Dot> searchedDots = new List<Dot>();
            List<Collision> collisions = new List<Collision>();

            int lastRecordIndex = dot.LocationSwapHistory.Count - swapHistoryLength;
            lastRecordIndex = Math.Max(lastRecordIndex, 0);
            var lastRecord = dot.LocationSwapHistory[lastRecordIndex];
            int lastRecordTime = lastRecord.Time;

            for (var i = dot.Collisions.Count - 1; i >= 0; --i)
            {
                var collision = dot.Collisions[i];

                // The below line from the original code doesn't make sense
                // as 'o' is not used and it's trying to use an index that 
                // may be out of range for 'searchedDots'
                // Commenting it out for clarity
                // var o = searchedDots[i];
            }

            return new List<Dot>();
        }
    }
}