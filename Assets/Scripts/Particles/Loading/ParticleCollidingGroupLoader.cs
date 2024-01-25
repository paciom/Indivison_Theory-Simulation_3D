using System.Collections.Generic;
using System.Linq;
using Frozen;
using Models;
using UnityEngine;
using Zenject;

namespace Particles.Loading
{
    public class ParticleCollidingGroupLoader : IParticleCollidingGroupLoader
    {
        private readonly IParticleRotator _particleRotator;
        private readonly IParticleLoader _particleLoader;
        private readonly ILargeParticlesLoader _largeParticlesLoader;

        [Inject]
        public ParticleCollidingGroupLoader
        (
            IParticleRotator particleRotator,
            IParticleLoader particleLoader,
            ILargeParticlesLoader largeParticlesLoader
        )
        {
            _particleRotator = particleRotator;
            _particleLoader = particleLoader;
            _largeParticlesLoader = largeParticlesLoader;
        }
        
        public void LoadLargestParticles(int numberOfLargestParticles,   Dictionary<int, Dot> dots)
        {
            var largestParticles = _largeParticlesLoader.Load(numberOfLargestParticles).ToArray();
            var randomLargestParticle = largestParticles[Random.Range(0, largestParticles.Length)];
            //foreach (var particle in largestParticles)
            {
                int particleCount= Random.Range(3, 200);
                int distance = Random.Range(4, 60);
                for (int i = 0; i < particleCount; i++)
                {
                    var rotatedParticle = _particleRotator.Rotate(randomLargestParticle, new Vector3(), Random.onUnitSphere, Random.Range(0, 360));
                    var movedParticle = MoveMassCenterBack(rotatedParticle, -distance);
                    _particleLoader.LoadParticle(movedParticle, dots);
                }

            }
        }

        private FrozenParticle MoveMassCenterBack(FrozenParticle particle, float distance)
        {
            // Move the whole particle's mass center back by distance, the direction of movement is opposite to the particle direction (average direction of all dots)
            Vector3 averageDirection = GetAverageDirection(particle.Dots);
            var movementDirection = -averageDirection;
            var movement = movementDirection.normalized * distance;
            var movedParticle = new FrozenParticle
            {
                Id = particle.Id,
                Dots = particle.Dots.Select(frozenDot => new FrozenDot
                {
                    Id = frozenDot.Id,
                    Position = frozenDot.Position - movement,
                    Direction = frozenDot.Direction,
                    ConnectedDotId = frozenDot.ConnectedDotId,
                }).ToList(),
            };
            return movedParticle;
            
        }

        private Vector3 GetAverageDirection(List<FrozenDot> particleDots)
        {
            var averageDirection = Vector3.zero;
            foreach (var frozenDot in particleDots)
            {
                averageDirection += frozenDot.Direction;
            }

            averageDirection /= particleDots.Count;
            return averageDirection;
        }
    }
}