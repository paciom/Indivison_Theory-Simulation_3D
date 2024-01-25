using System.Linq;
using Models;

namespace Particles
{
    public class ParticleNamer : IParticleNamer
    {
        public string GetId(Particle particle)
        {
            return string.Join(",", particle.Dots.Distinct().Select(e => e.ToString()));
        }
    }
}