using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Particles
{
    public class ParticlePatternChecker : IParticlePatternChecker
    {
        private readonly IAppStateFactory _appStateFactory;

        [Inject]
        public ParticlePatternChecker(IAppStateFactory appStateFactory)
        {
            _appStateFactory = appStateFactory;
        }

        public bool IsPatternsValid(List<int> dotIds, Dictionary<int, List<int>> patterns)
        {
            bool everyDotHasPattern = dotIds.Select(id => patterns.ContainsKey(id) ? patterns[id] : null)
                .All(pattern => pattern != null && pattern.Any());

            if (!everyDotHasPattern)
            {
                return false;
            }
            var allDots = _appStateFactory.State.Data.Dots;
            var connectedDots = dotIds.Select(id => allDots[id].ConnectedDotId).ToArray();
            var allConnectedDotsInPattern = connectedDots.All(dot => dot!=null&&  dotIds.Contains(dot!.Value));
            
            return allConnectedDotsInPattern;
        }
    }
}