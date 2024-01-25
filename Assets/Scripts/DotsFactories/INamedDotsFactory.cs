using System.Collections.Generic;
using Models;

namespace DotsFactories
{
    public interface INamedDotsFactory
    {
        Dictionary<int, Dot>  CreateDots(DotsFactoryType type);
    }
}