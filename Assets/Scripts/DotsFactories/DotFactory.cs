using Collisions;
using Models;
using UnityEngine;
using Zenject;

namespace DotsFactories
{
    public class DotFactory : IDotFactory
    {
        public float radius=1;

        [Inject]
        public DotFactory()
        {
        }
        public Dot CreateDot(int id, Vector3 location, Vector3 direction)
        {
 
            var dotColor = UnityEngine.Random.ColorHSV();
            var dotGenerator = GameObject.Find("DotGenerator");
            var generator = dotGenerator.GetComponent<DotGenerator>();
            var dotPrefab = generator.dotPrefab;

            GameObject dotObj = UnityEngine.Object. Instantiate(dotPrefab, location, Quaternion.identity);
            Dot dot = dotObj.GetComponent<Dot>();
            dot.Id = id;
            dot.Initialize(dotColor, location, radius);
            dot.Direction = direction;
            dot.Speed = 1;
            return dot;
        }
    }
}