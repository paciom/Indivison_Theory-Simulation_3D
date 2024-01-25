using Models;

namespace Particles
{
    public interface IParticleNamer
    {
        string GetId(Particle particle);
    }
}