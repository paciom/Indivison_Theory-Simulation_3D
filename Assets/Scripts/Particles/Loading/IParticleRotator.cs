using Frozen;
using UnityEngine;

namespace Particles.Loading
{
    public interface IParticleRotator
    {
        FrozenParticle Rotate(FrozenParticle particle, Vector3 rotationAxisPoint, Vector3 rotationAxisDirection,
            float rotationAngleOfRightHandRule);
    }
}