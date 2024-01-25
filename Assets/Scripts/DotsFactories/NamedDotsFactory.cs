using System;
using System.Collections.Generic;
using Models;
using Zenject;

namespace DotsFactories
{
    public class NamedDotsFactory : INamedDotsFactory
    {
        readonly IQuatonFactory _quatonFactory;
        private readonly IRandomDotListFactory _randomDotListFactory;
        private readonly ICenterCollidingDotsFactory _centerCollidingDotsFactory;

        [Inject]
        public NamedDotsFactory
        (
            IQuatonFactory quatonFactory,
            IRandomDotListFactory randomDotListFactory,
            ICenterCollidingDotsFactory centerCollidingDotsFactory
        )
        {
            _quatonFactory = quatonFactory;
            _randomDotListFactory = randomDotListFactory;
            _centerCollidingDotsFactory = centerCollidingDotsFactory;
        }
    
        public Dictionary<int, Dot> CreateDots(DotsFactoryType type)
        {
            switch (type)
            {
                case DotsFactoryType.Random:
                    return _randomDotListFactory.CreateDots();
                case DotsFactoryType.Quaton:
                    return _quatonFactory.CreateDots();
                case DotsFactoryType.CenterColliding:
                    return _centerCollidingDotsFactory.CreateDots();
                default:
                    throw new Exception("Unknown type");
            }
        }
    }
}