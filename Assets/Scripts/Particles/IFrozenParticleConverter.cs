using Frozen;
using Models;

namespace Particles
{
    public interface IFrozenParticleConverter
    {
        FrozenParticle ConvertToFrozenParticle(Particle particle);
    }
}