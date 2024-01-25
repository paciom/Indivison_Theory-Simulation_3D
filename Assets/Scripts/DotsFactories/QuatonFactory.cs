using System.Collections.Generic;
using Models;
using UnityEngine;
using Zenject;

namespace DotsFactories
{
    public class QuatonFactory : IQuatonFactory
    {
        private readonly IDotFactory _dotFactory;

        [Inject]
        public QuatonFactory(IDotFactory dotFactory)
        {
            _dotFactory = dotFactory;
        }

        public Dictionary<int, Dot> CreateDots()
        {
            Dictionary<int, Dot> dots = new()
            {
                { 0, _dotFactory.CreateDot(0, new Vector3(5, 0, 0), new Vector3(1, 0, 0)) },
                { 1, _dotFactory.CreateDot(1, new Vector3(25, 0, 0), new Vector3(1, 0, 0)) },
                { 2, _dotFactory.CreateDot(2, new Vector3(31, 0, 0), new Vector3(-1, 0, 0)) },
                { 3, _dotFactory.CreateDot(3, new Vector3(37, 0, 0), new Vector3(-1, 0, 0)) }
            };

            return dots;
        }
    }
}