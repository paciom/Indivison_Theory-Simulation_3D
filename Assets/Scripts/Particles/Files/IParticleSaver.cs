using System.Collections.Generic;
using System.IO;
using Frozen;

namespace Particles.Files
{
    public interface IParticleSaver
    {
        void Save();
        void SaveParticle(FrozenParticle particle);
        List<FileInfo> GetParticleFiles();
        FrozenParticle LoadFrozenParticle(FileInfo particleFile);
    }
}