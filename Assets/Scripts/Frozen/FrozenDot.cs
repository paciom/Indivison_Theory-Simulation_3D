using System;
using UnityEngine;

namespace Frozen
{
    
    [Serializable]
    public class FrozenDot
    {
        public Vector3 Position;
        public Vector3 Direction;
        public int Id;
        public int ConnectedDotId;
    }
}