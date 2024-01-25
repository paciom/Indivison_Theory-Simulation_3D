using System;
using System.Collections.Generic;
using System.Linq;
using Frozen;
using Models;
using Particles.Files;
using Zenject;

namespace Particles
{
    public class ParticleRecorder : IParticleRecorder
    {
        private readonly IAppStateFactory _appStateFactory;
        private readonly IParticleFinder _particleFinder;
        private readonly IParticleSaver _particleSaver;
        private readonly IFrozenParticleConverter _frozenParticleConverter;
        AppState State => _appStateFactory.State;

        [Inject]
        public ParticleRecorder
        (
            IAppStateFactory appStateFactory,
            IParticleFinder particleFinder,
            IParticleSaver particleSaver,
            IFrozenParticleConverter frozenParticleConverter
        )
        {
            _appStateFactory = appStateFactory;
            _particleFinder = particleFinder;
            _particleSaver = particleSaver;
            _frozenParticleConverter = frozenParticleConverter;
        }

        public void AddFrozenParticles()
        {
            if (State.Data.Time != 0 && State.Data.Time % State.Ui.ParticleFindingInterval == 0)
            {
                var particles = _particleFinder.FindParticles(State.Data.Dots);
                foreach (var particle in particles)
                {
                    FrozenParticle frozenParticle = _frozenParticleConverter.ConvertToFrozenParticle(particle);
                    if (State.FrozenParticles.ContainsKey(particle.Id))
                    {
                        State.FrozenParticles[particle.Id] = frozenParticle;
                    }
                    else
                    {
                        if (State.FrozenParticles.TryAdd(particle.Id, frozenParticle))
                        {
                            if (frozenParticle.Dots.Count > 4)
                            {
                                _particleSaver.SaveParticle(frozenParticle);
                                if (frozenParticle.Dots.Count > State.LargestParticleCount)
                                {
                                    State.LargestParticleCount = frozenParticle.Dots.Count;
                                }
                            }
                        }
                    }
                }

                PrintParticles(particles);
            }
        }


        public void PrintParticles(List<Particle> particles)
        {
            var particleSizes = State.FrozenParticles.Values
                .Select(e => e.Dots.Count)
                .Where(element => element > 1)
                .ToList();

            particleSizes.Sort((a, b) => b - a);
            Console.WriteLine($"{string.Join(", ", particleSizes)}");
        }
    }
}