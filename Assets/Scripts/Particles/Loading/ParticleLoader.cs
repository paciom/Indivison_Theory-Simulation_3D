using System.Collections.Generic;
using System.Linq;
using DotsFactories;
using Frozen;
using Models;
using Zenject;

namespace Particles.Loading
{
    public class ParticleLoader : IParticleLoader
    {
        private readonly IAppStateFactory _appStateFactory;
        private readonly IDotFactory _dotFactory;

        [Inject]
        public ParticleLoader(IAppStateFactory appStateFactory, IDotFactory dotFactory)
        {
            _appStateFactory = appStateFactory;
            _dotFactory = dotFactory;
        }

        public void LoadParticle(FrozenParticle particle, Dictionary<int, Dot> dots)
        {
            int startDotId = dots.Count;
            int currentDotId = startDotId;

            var dotIds = new List<int>();
            foreach (var frozenDot in particle.Dots)
            {
                var dot = _dotFactory.CreateDot(currentDotId, frozenDot.Position, frozenDot.Direction);
                int frozenDotConnectedDotId = frozenDot.ConnectedDotId;
                dot.ConnectedDotId  = frozenDotConnectedDotId + startDotId;
            
                dots[currentDotId] = dot;
                dotIds.Add(currentDotId);
                currentDotId++;
            }
            
            particle.Id = string.Join(",",dotIds.OrderBy(id=>id));
            _appStateFactory.State.FrozenParticles[particle.Id] = particle;

        }
    }
}