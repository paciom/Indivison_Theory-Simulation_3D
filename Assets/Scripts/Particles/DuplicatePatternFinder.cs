using System.Collections.Generic;
using Collisions;
using Zenject;

namespace Particles
{
    public class DuplicatePatternFinder : IDuplicatePatternFinder
    {
        [Inject]
        public DuplicatePatternFinder()
        {
            
        }
        public bool IsRepeat(List<CollisionRecord> ids, int bigPatternLength, int smallPatternLength)
        {
            if (bigPatternLength % smallPatternLength != 0)
            {
                return false;
            }

            for (var i = 0; i < bigPatternLength; ++i)
            {
                var lastIndex = ids.Count - 1;
                var bigPatternItem = ids[lastIndex - i];
                var smallPatternItem = ids[lastIndex - (i % smallPatternLength)];
                if (bigPatternItem.DotId != smallPatternItem.DotId)
                {
                    return false;
                }
            }

            return true;
        }
    }
}