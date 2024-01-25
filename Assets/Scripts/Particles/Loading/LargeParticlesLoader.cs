using System.Collections.Generic;
using System.IO;
using System.Linq;
using Frozen;
using Particles.Files;
using Zenject;


namespace Particles.Loading
{
    public class LargeParticlesLoader : ILargeParticlesLoader
    {
        private readonly IParticleSaver _particleSaver;

        [Inject]
        public LargeParticlesLoader
        (
            IParticleSaver particleSaver
        )
        {
            _particleSaver = particleSaver;
        }

        public IEnumerable<FrozenParticle> Load(int numberOfLargestParticles)
        {
            // Load the largest particles from the frozen particles from particle files with file names ending with "FrozenParticles.json" which are saved using class ParticleSaver
            // Load all frozen particles from all particle files with file names ending with "FrozenParticles.json" using class ParticleSaver
            var frozenParticles = new List<FrozenParticle>();
            var particleFiles = _particleSaver.GetParticleFiles().OrderByDescending(file=>int.Parse(file.Name.Substring(0, 3))).Take(numberOfLargestParticles).ToList();
            foreach (var particleFile in particleFiles)
            {
                FrozenParticle frozenParticle = _particleSaver.LoadFrozenParticle(particleFile);
                frozenParticles.Add(frozenParticle);
            }

            return frozenParticles;
            // // Sort the frozen particles by size (number of dots)
            // frozenParticles.Sort((p1, p2) => p1.Dots.Count.CompareTo(p2.Dots.Count));
            //
            // // Get the largest particles
            // var largestParticles = frozenParticles.GetRange(frozenParticles.Count - numberOfLargestParticles,
            //     numberOfLargestParticles);
            //
            //
            // return largestParticles;
        }
    }
}