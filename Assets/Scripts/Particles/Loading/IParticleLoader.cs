using System.Collections.Generic;
using Frozen;
using Models;

namespace Particles.Loading
{
    public interface IParticleLoader
    {
        void LoadParticle(FrozenParticle particle, Dictionary<int, Dot> dots);
    }
}