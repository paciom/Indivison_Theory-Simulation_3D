using System.Collections.Generic;
using System.Linq;
using Models;

namespace Particles
{
    public class ParticleFreezer
    {
        public Particle Freeze(Particle particle)
        {
            var result = new Particle
            {
                Dots = new List<int>(particle.Dots.Select(id => id)),
                Id = particle.Id
            };

            return result;
        }
    }
}