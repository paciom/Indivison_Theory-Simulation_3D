using System;
using System.Collections.Generic;
using System.Linq;
using Frozen;
using Models;
using UnityEngine;
using Zenject;

namespace Particles
{
    public class FrozenParticleConverter : IFrozenParticleConverter
    {
        private readonly IAppStateFactory _appStateFactory;

        [Inject]
        public FrozenParticleConverter(IAppStateFactory appStateFactory)
        {
            _appStateFactory = appStateFactory;
        }

        public FrozenParticle ConvertToFrozenParticle(Particle particle)
        {
            var frozenParticle = new FrozenParticle
            {
                Id = particle.Id,
                Dots = new List<FrozenDot>(),
            };
            var newDotId = 0;
            var dotIdMap = new Dictionary<int, int>();
            var dots = particle.Dots.OrderBy(id => id);
            var state =_appStateFactory.State;
            var massCenter = GetMassCenter(particle, state);
            foreach (var dotId in dots)
            {
                var dot = state.Data.Dots[dotId];
                var frozenDot = new FrozenDot
                {
                    Id = newDotId,
                    Position = dot.Position - massCenter, // Center the particle to origin point
                    Direction = dot.Direction,
                };
                frozenParticle.Dots.Add(frozenDot);

                dotIdMap.Add(dotId, newDotId);
                newDotId++;
            }

            for (int i = 0; i < particle.Dots.Count; i++)
            {
                var dotId = particle.Dots[i];
                var connectedOriginalDotId = state.Data.Dots[dotId].ConnectedDotId;
                if (connectedOriginalDotId != null)
                {
                    frozenParticle.Dots[i].ConnectedDotId = dotIdMap[connectedOriginalDotId.Value];
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            return frozenParticle;
        }

        private static Vector3 GetMassCenter(Particle particle, AppState state)
        {
            var massCenter = particle.Dots.Select(id => state.Data.Dots[id].Position).Aggregate((a, b) => a + b) /
                             particle.Dots.Count;
            return massCenter;
        }
    }
}