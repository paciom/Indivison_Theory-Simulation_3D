using System.Collections.Generic;
using Models;

namespace Particles
{
    public interface ICollisionDotsFinder
    {
        List<Dot> FindCollidedDots(int startingDotId, List<int> searchedList, int time);
    }
}