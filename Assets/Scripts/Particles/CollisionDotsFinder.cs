using System.Collections.Generic;
using Models;
using Zenject;

namespace Particles
{
    public class CollisionDotsFinder : ICollisionDotsFinder
    {
        private readonly IAppStateFactory _appStateFactory;

        [Inject]
        public CollisionDotsFinder(IAppStateFactory appStateFactory)
        {
            _appStateFactory = appStateFactory;
        }

        public List<Dot> FindCollidedDots(int startingDotId, List<int> searchedList, int time)
        {
            if (searchedList.Contains(startingDotId))
            {
                return new List<Dot>();
            }
            else
            {
                searchedList.Add(startingDotId);
                List<Dot> results = new List<Dot>();

                if (!_appStateFactory.State.Data.Dots.TryGetValue(startingDotId, out Dot dot))
                {
                    return results;
                }

                for (int i = dot.Collisions.Count - 1; i >= 0; --i)
                {
                    var collidedDot = dot.Collisions[i];
                    if (collidedDot.Time < time)
                    {
                        break;
                    }
                    var otherCollidedDots = FindCollidedDots(collidedDot.DotId, searchedList, time);
                    results.AddRange(otherCollidedDots);
                }
                return results;
            }
        }
    }
}