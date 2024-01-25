using System.Collections.Generic;
using Models;
using Particles.Loading;
using Zenject;

namespace DotsFactories
{
    public class CenterCollidingDotsFactory : ICenterCollidingDotsFactory
    {
        private readonly IParticleCollidingGroupLoader _particleCollidingGroupLoader;

        [Inject]
        public CenterCollidingDotsFactory(IParticleCollidingGroupLoader particleCollidingGroupLoader)
        {
            _particleCollidingGroupLoader = particleCollidingGroupLoader;
        }

        public Dictionary<int, Dot> CreateDots()
        {
            Dictionary<int, Dot> dots = new();
            _particleCollidingGroupLoader.LoadLargestParticles(20, dots);
            return dots;
        }
    }
}