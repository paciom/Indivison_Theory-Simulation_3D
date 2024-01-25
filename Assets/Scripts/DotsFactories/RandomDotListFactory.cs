using System.Collections.Generic;
using Models;
using Zenject;

namespace DotsFactories
{
    public class RandomDotListFactory : IRandomDotListFactory
    {
        private readonly IAppStateFactory _appStateFactory;
        private readonly IDotFactory _dotFactory;

        [Inject]
        public RandomDotListFactory(IAppStateFactory appStateFactory, IDotFactory dotFactory)
        {
            _appStateFactory = appStateFactory;
            _dotFactory = dotFactory;
        }

        public Dictionary<int, Dot> CreateDots()
        {
            Dictionary<int, Dot> dots = new ();
            for (int i = 0; i < _appStateFactory.State.Data.NumberOfDots; i++)
            {
                var dot = _dotFactory.CreateDot(i, UnityEngine.Random.insideUnitSphere * 0.5f, UnityEngine.Random.onUnitSphere);
                //GameObject dotObj = Instantiate(dotPrefab, UnityEngine.Random.insideUnitSphere * 10, Quaternion.identity);
                //Dot dot = dotObj.GetComponent<Dot>();
                //dot.Initialize(UnityEngine.Random.ColorHSV(), dotObj.transform.position, radius);
                dots[i]=dot;
            }

            return dots;
        }
    }
}