using System.Collections.Generic;

namespace Models
{
    public class Particle
    {
        // If these properties are overriding properties in a base class, the base properties must be marked virtual or abstract.
        public List<int> Dots = new List<int>();
        public string Id = string.Empty;

        // Constructors can be added if needed. This class uses the default constructor.
    }
}
