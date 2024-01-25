using System.Collections.Generic;
using Models;

namespace Particles.Loading
{
    public interface IParticleCollidingGroupLoader
    {
        void LoadLargestParticles(int numberOfLargestParticles, Dictionary<int, Dot> dots);
    }
}