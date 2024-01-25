using System.Collections.Generic;
using System.Linq;
using Models;
using Zenject;

namespace Particles
{
    public class ParticleFinder : IParticleFinder
    {
        private readonly IPatternFinder _patternFinder;
        private readonly ICollisionDotsFinder _collisionDotsFinder;
        private readonly IParticleGrouper _particleGrouper;
        private readonly IParticlePatternChecker _particlePatternChecker;

        [Inject]
        public ParticleFinder(
            IPatternFinder patternFinder,
            ICollisionDotsFinder collisionDotsFinder,
            IParticleGrouper particleGrouper,
            IParticlePatternChecker particlePatternChecker
        )
        {
            _patternFinder = patternFinder;
            _collisionDotsFinder = collisionDotsFinder;
            _particleGrouper = particleGrouper;
            _particlePatternChecker = particlePatternChecker;
        }

        public List<Particle> FindParticles(Dictionary<int, Dot> dots)
        {
            Dictionary<string, Particle> particleMap = new Dictionary<string, Particle>();
            Dictionary<int, List<int>> dotCollisionPatterns = new Dictionary<int, List<int>>();

            foreach (var dot in dots.Values)
            {
                List<int> patternDotIds = new List<int>();

                var collisions = dot.Collisions;
                var patterns = _patternFinder.FindPattern(collisions);
                dotCollisionPatterns[dot.Id] = patterns;

                if (patterns.Any())
                {
                    var longestPattern = patterns.Last();
                    var patternCollisions = collisions.Skip(collisions.Count - 1 - longestPattern).Take(longestPattern).ToList();
                    var patternStartTime = patternCollisions.First().Time;

                    patternDotIds.AddRange(patternCollisions.Select(collision => collision.DotId));

                    List<int> searchedDots = new List<int> { dot.Id };
                    List<int> dotsInParticle = patternDotIds.ToList();

                    foreach (var dotId in patternDotIds)
                    {
                        var collidedDots = _collisionDotsFinder.FindCollidedDots(dotId, searchedDots, patternStartTime);
                        dotsInParticle.AddRange(collidedDots.Select(d => d.Id));
                    }

                    var particle = new Particle
                    {
                        Dots = new List<int> { dot.Id }
                    };
                    particle.Dots.AddRange(dotsInParticle);
                    particle.Dots.Sort();

                    var particleId = string.Join(",", particle.Dots.Distinct());

                    if (!particleMap.ContainsKey(particleId))
                    {
                        particleMap[particleId] = particle;
                    }
                }
            }

            var particles = _particleGrouper.GroupParticles(particleMap.Values.ToList());

            var validParticles = particles
                .Where(particle => _particlePatternChecker.IsPatternsValid(particle.Dots, dotCollisionPatterns))
                .ToList();

            return validParticles;
        }
    }
}