using System.Collections.Generic;
using Frozen;

namespace Particles.Loading
{
    public interface ILargeParticlesLoader
    {
        IEnumerable<FrozenParticle> Load(int numberOfLargestParticles);
    }
}