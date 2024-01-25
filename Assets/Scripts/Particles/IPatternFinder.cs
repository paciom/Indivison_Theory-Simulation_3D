using System.Collections.Generic;
using Collisions;

namespace Particles
{
    public interface IPatternFinder
    {
        List<int> FindPattern(List<CollisionRecord> ids);
    }
}