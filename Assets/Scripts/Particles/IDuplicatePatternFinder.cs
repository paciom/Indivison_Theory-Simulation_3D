using System.Collections.Generic;
using Collisions;

namespace Particles
{
    public interface IDuplicatePatternFinder
    {
        bool IsRepeat(List<CollisionRecord> ids, int bigPatternLength, int smallPatternLength);
    }
}