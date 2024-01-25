using System.Collections.Generic;
using System.IO;
using System.Linq;
using Frozen;
using UnityEngine;
using Zenject;

namespace Particles.Files
{
    public class ParticleSaver : IParticleSaver
    {
        private readonly IAppStateFactory _appStateFactory;

        [Inject]
        public ParticleSaver(IAppStateFactory appStateFactory)
        {
            _appStateFactory = appStateFactory;
        }
        
        public void Save()
        {
            // Save Particles
            foreach (var particle in _appStateFactory.State.FrozenParticles.Values)
            {
                if (particle.Dots.Count > 3)
                {
                    SaveParticle(particle);
                }
            }
        }


        public void SaveParticle(FrozenParticle particle)
        {
            // Serialize particle and save to a file  if the file does not exist already, use the particle id as the file name, If the file exists, do nothing

            var particleId = particle.Id.Replace(',', '_');
            // Create file name that starts with particle dot count and ends with particle id, separated by underscore. The particle dot count should be padded with 0 to make it 3 digits
            var fileName = $"{particle.Dots.Count:D3}_{particleId}.particle3d.json";

            var filePath = GetFolder() + Path.DirectorySeparatorChar + fileName;
            if (File.Exists(filePath))
            {
                Debug.Log($"File {fileName} already exists");
                return;
            }

            var particleJson = JsonUtility.ToJson(particle);
            File.WriteAllText(filePath, particleJson);
        }

        private string GetFolder()
        {
            // Each time the app is started, create a new folder with timestamp as folder name to storße the particles.
            // We should use the folder all through the app life time. But next time the app starts, create a new folder with timestamp
            var folderPath = GetSavedParticleFolder() + _appStateFactory.State.FrozenParticleFolder;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }

        private static string GetSavedParticleFolder()
        {
            return Application.dataPath + "/SavedParticles/";
        }

        public List<FileInfo> GetParticleFiles()
        {
            var folder = GetSavedParticleFolder();
            // List all files in the folder that end with "FrozenParticles.json"
            var particleFiles = new List<FileInfo>();
            var directoryInfo = new DirectoryInfo(folder);
            // Get all files in sub folders
            var files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.Name.EndsWith(".particle3d.json"))
                {
                    particleFiles.Add(file);
                }
            }
            return particleFiles.ToList();
        }

        public FrozenParticle LoadFrozenParticle(FileInfo particleFile)
        {
            // Write C# load the frozen particle from the file as json format
            var particleJson = File.ReadAllText(particleFile.FullName);
            var frozenParticle = JsonUtility.FromJson<FrozenParticle>(particleJson);
            return frozenParticle;
        }
    }
}