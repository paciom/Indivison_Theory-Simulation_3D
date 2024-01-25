using System.Collections.Generic;

namespace Particles
{
    public interface IParticlePatternChecker
    {
        bool IsPatternsValid(List<int> dotIds, Dictionary<int, List<int>> patterns);
    }
}