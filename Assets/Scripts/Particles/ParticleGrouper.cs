using System.Collections.Generic;
using System.Linq;
using Models;
using Zenject;

namespace Particles
{
    public class ParticleGrouper : IParticleGrouper
    {
        private readonly IParticleNamer _namer;

        [Inject]
        public ParticleGrouper(IParticleNamer namer)
        {
            _namer = namer;
        }

        public List<Particle> GroupParticles(List<Particle> partialGroups)
        {
            List<Particle> result = new List<Particle>();

            foreach (var group in partialGroups)
            {
                var newParticle = new Particle { Dots = new List<int>(group.Dots) }; // Clone to avoid altering the original list
                List<Particle> toMerge = new List<Particle>();

                foreach (var existingGroup in result)
                {
                    if (newParticle.Dots.Any(item => existingGroup.Dots.Contains(item)))
                    {
                        toMerge.Add(existingGroup);
                    }
                }

                foreach (var mergeGroup in toMerge)
                {
                    newParticle.Dots.AddRange(mergeGroup.Dots);
                    result.Remove(mergeGroup);
                }

                // Remove duplicates and sort
                newParticle.Dots = RemoveDuplicated(newParticle.Dots).ToList();
                newParticle.Dots.Sort();
                newParticle.Id = _namer.GetId(newParticle);
                result.Add(newParticle);
            }

            return result;
        }

        private IEnumerable<int> RemoveDuplicated(List<int> dots)
        {
            HashSet<int> ids = new HashSet<int>();
            List<int> distinctDots = new List<int>();

            foreach (var dotId in dots)
            {
                if (!ids.Contains(dotId))
                {
                    ids.Add(dotId);
                    distinctDots.Add(dotId);
                }
            }

            return distinctDots;
        }
    }
}