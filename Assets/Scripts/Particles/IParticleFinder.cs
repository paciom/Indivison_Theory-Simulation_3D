using System.Collections.Generic;
using Models;

namespace Particles
{
    public interface IParticleFinder
    {
        List<Particle> FindParticles(Dictionary<int, Dot> dots);
    }
}