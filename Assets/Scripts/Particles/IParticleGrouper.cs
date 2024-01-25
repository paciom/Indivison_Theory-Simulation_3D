using System.Collections.Generic;
using Models;

namespace Particles
{
    public interface IParticleGrouper
    {
        List<Particle> GroupParticles(List<Particle> partialGroups);
    }
}