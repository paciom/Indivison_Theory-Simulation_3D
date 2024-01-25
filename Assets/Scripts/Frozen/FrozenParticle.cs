using System;
using System.Collections.Generic;

namespace Frozen
{
    [Serializable]
    public class FrozenParticle
    {
        public List<FrozenDot> Dots = new();
        public string Id = string.Empty;
    }
}
