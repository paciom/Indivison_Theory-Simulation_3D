using Models;

namespace Collisions
{
    public class Collision
    {
        public Dot A { get; }
        public Dot B { get; }
        public double Distance { get; }  // 'num' in Dart is most similar to 'double' in C#

        public Collision(Dot a, Dot b, double distance)
        {
            A = a;
            B = b;
            Distance = distance;
        }
    }
}
