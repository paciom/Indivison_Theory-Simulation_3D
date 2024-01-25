using System.Collections.Generic;

namespace Models
{
    public class DotData
    {
        public double InitialAreaWidth = 4;
        public double InitialAreaHeight = 4;
        public double CollisionRadius = 0.5;
        public int Seed = 0;
        public int NumberOfDots = 500;
        public int Time = 0;

        // Initializes the dictionary. Equivalent to Dart's Map.s
        public Dictionary<int, Dot> Dots = new Dictionary<int, Dot>();

        // Constructors can be added if needed. This class uses the default constructor.
    }
}
