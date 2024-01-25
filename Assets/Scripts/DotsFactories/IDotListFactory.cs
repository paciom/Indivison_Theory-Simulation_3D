using System.Collections.Generic;
using Models;

namespace DotsFactories
{
    public interface IDotListFactory
    {
        Dictionary<int, Dot> CreateDots();
    }
}