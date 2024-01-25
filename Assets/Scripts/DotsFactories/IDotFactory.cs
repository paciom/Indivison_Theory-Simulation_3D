using Models;
using UnityEngine;

namespace DotsFactories
{
    public interface IDotFactory
    {
        Dot CreateDot(int id, Vector3 location, Vector3 direction);
    }
}