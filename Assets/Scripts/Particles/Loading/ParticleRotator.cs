using System.Collections.Generic;
using Frozen;
using UnityEngine;
using Zenject;

namespace Particles.Loading
{
    public class ParticleRotator : IParticleRotator
    {
        [Inject]
        public ParticleRotator()
        {
        }

        
        public FrozenParticle Rotate(FrozenParticle particle, Vector3 rotationAxisPoint, Vector3 rotationAxisDirection,
            float rotationAngleOfRightHandRule)
        {
            // Write C#
            var rotatedParticle = new FrozenParticle
            {
                Id = particle.Id,
                Dots = new List<FrozenDot>(),
            };
            foreach (var dot in particle.Dots)
            {
                var rotatedDot = RotateDot(dot, rotationAxisPoint, rotationAxisDirection, rotationAngleOfRightHandRule);
                rotatedParticle.Dots.Add(rotatedDot);
            }

            return rotatedParticle;
        }

        private FrozenDot RotateDot(FrozenDot dot, Vector3 rotationAxisPoint, Vector3 rotationAxisDirection, float rotationAngleOfRightHandRule)
        {
// Write C#
            var rotatedDot = new FrozenDot
            {
                Id = dot.Id,
                Position = RotateVector(dot.Position, rotationAxisPoint, rotationAxisDirection, rotationAngleOfRightHandRule),
                Direction = RotateVector(dot.Direction, rotationAxisPoint, rotationAxisDirection, rotationAngleOfRightHandRule),
                ConnectedDotId = dot.ConnectedDotId,
            };
            return rotatedDot;
            
        }

        private Vector3 RotateVector(Vector3 dotPosition, Vector3 rotationAxisPoint, Vector3 rotationAxisDirection, float rotationAngleOfRightHandRule)
        {
            // Write C#
            var rotatedVector = Quaternion.AngleAxis(rotationAngleOfRightHandRule, rotationAxisDirection) * (dotPosition - rotationAxisPoint) + rotationAxisPoint;
            return rotatedVector;
        }
    }
}