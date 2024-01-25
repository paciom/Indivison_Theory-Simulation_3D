using System;
using System.Collections.Generic;
using Frozen;

namespace Models
{
    public class AppState
    {
        public DotData Data  = new DotData();

        public UiState Ui = new UiState();
    
        public int LargestParticleCount = 1;
        public String FrozenParticleFolder = DateTimeOffset.Now.ToString("yyyy-MM-dd_HH-mm-ss-fffff");

        public Dictionary<string, FrozenParticle> FrozenParticles { get; set; } = new();
    }
}