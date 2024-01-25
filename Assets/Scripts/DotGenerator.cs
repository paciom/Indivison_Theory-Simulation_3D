using System;
using System.Collections.Generic;
using System.Linq;
using DotsFactories;
using Models;
using Particles;
using Particles.Files;
using Particles.Loading;
using SceneOperations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Collisions
{
    public class DotGenerator : MonoBehaviour
    {
        public float radius;
        public GameObject dotPrefab;

        private ICollisionChecker _collisionChecker;
        private IParticleRecorder _particleRecorder;
        private IAppStateFactory _appStateFactory;
        private IParticleSaver _particleSaver;
        private INamedDotsFactory _namedDotsFactory;
        ISceneResetter _sceneResetter;
    
        [Inject]
        public void Construct
        (
            IParticleRecorder particleRecorder,
            IAppStateFactory appStateFactory,
            ICollisionChecker collisionChecker,
            IParticleSaver particleSaver,
            INamedDotsFactory namedDotsFactory,
            ISceneResetter sceneResetter
        )
        {
            _particleRecorder = particleRecorder;
            _appStateFactory = appStateFactory;
            _collisionChecker = collisionChecker;
            _particleSaver = particleSaver;
            _namedDotsFactory = namedDotsFactory;
            _sceneResetter = sceneResetter;
        }
    
        private void Start()
        {
            //var  dots = CreateQuaton();
            _appStateFactory.State.Data.Seed = Random.Range(0, 100000000);
            UnityEngine.Random.InitState(_appStateFactory.State.Data.Seed);
        
            var dots = _namedDotsFactory.CreateDots(DotsFactoryType.Random);
            for (int i = 0; i < dots.Count; i++)
            {
                _appStateFactory.State.Data.Dots[i] = dots[i];
            }


            // var particleFinder = new ParticleFinder
            // (
            //     new PatternFinder(new DuplicatePatternFinder()),
            //     new CollisionDotsFinder(_appStateFactory.State),
            //     new ParticleGrouper(new ParticleNamer()),
            //     new ParticlePatternChecker(_appStateFactory.State)
            // );
            // _particleRecorder = new ParticleRecorder(particleFinder);
        
            // float scale = 1.5f;
            // float speedScale = 0.5f;
            // var particle1 = CreateParticle(new Vector3(0, scale * 40, 0), 10, 100, 0);
            // var particle2 = CreateParticle(new Vector3(0, scale * 29, scale * 29), 10, 100, 0);
            //
            //
            // SetDirection(particle1, new Vector3(0, -1 * speedScale, 0));
            // SetDirection(particle2, new Vector3(0, -1 * speedScale, -1 * speedScale));
        }

        private void MoveDots()
        {
            Console.WriteLine("Move" + _appStateFactory.State.Data.Time);

            foreach (var dot in _appStateFactory.State.Data.Dots.Values)
            {
                dot.Position += dot.Direction;
            }
        }



        private void Update()
        {
            var state = _appStateFactory.State;
            state.Ui.ScaledTime++;
            if (state.Ui.ScaledTime % state.Ui.TimeScale !=0)
            {
                return;
            }
        
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Pause
                state.Ui.Running = false;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                // Continue
                state.Ui.Running = true;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                // Reverse the direction of points
                // This is to simulate "Particle collider" to make particles run towards each other to form larger particles.
                foreach (var dot in state.Data.Dots.Values)
                {
                    dot.Direction *= -1;
                }
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                // Put points in position opposite to origin point. 
                // This is to simulate "Particle collider" to make particles run towards each other to form larger particles.
                foreach (var dot in state.Data.Dots.Values)
                {
                    dot.Position *= -1;
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                _particleSaver.Save();
            }

            // Slow motion
            if (Input.GetKeyDown(KeyCode.M))
            {
                state.Ui.TimeScale = 100f;
            }


            if (Input.GetKeyDown(KeyCode.D))
            {
                _sceneResetter.EmptyScene(state);
                _appStateFactory.State.Data.Dots.Clear();
                _appStateFactory.State.Data.Dots = _namedDotsFactory.CreateDots(DotsFactoryType.CenterColliding);
            }

            if (Input.GetKeyDown(KeyCode.B)) // Run in the background
            {
                _appStateFactory.State.Ui.RunningInBackground = true;
            }



            if (state.Ui.Running)
            {
                if (state.Data.Time % 200 == 1)
                {
                    _sceneResetter.EmptyScene(state);
                    _appStateFactory.State.Data.Dots = _namedDotsFactory.CreateDots(DotsFactoryType.CenterColliding);
                }
                if (_appStateFactory.State.Ui.RunningInBackground)
                {
                    while (true)
                    {
                        Run(state);
                        if (Input.GetKeyDown(KeyCode.F)) // Run in the forground
                        {
                            _appStateFactory.State.Ui.RunningInBackground = false;
                        }
                    }
                }
                else
                {
                      Run(state);
                }
              


                RemoveOldHistoryLines();
            }
        }

        private void Run(AppState state)
        {
            state.Data.Time++;
            MoveDots();
            _collisionChecker.HandleCollisions();

            _particleRecorder.AddFrozenParticles();
        }

        private void RemoveOldHistoryLines()
        {
            var lines = _appStateFactory.State.Ui.HistoryLines;
            // Remove all the lines in _appStateFactory.State.Ui.HistoryLines which is older than 1000 (difference with _appStateFactory.State.Data.Time is bigger than 1000) in 
            var currentTime = _appStateFactory.State.Data.Time;
            var oldLines = lines.Where(line => currentTime - line.Time > 30).ToArray();
            foreach (var oldLine in oldLines)
            {
                Object.Destroy(oldLine.LineRender);
                lines.Remove(oldLine);
            }
        }


        public List<Dot> CreateParticle(Vector3 massCenterLocation, float radius, int numberOfDots, float speed)
        {
            // Calculate the position range for the dots around the mass center
            float maxDistanceFromCenter = radius;
            float minDistanceFromCenter = radius * 0.5f;
            Vector3 randomOffsetRange = new Vector3(maxDistanceFromCenter, maxDistanceFromCenter, maxDistanceFromCenter);

            // Calculate the average speed for the dots
            float averageSpeed = speed;

            // Generate random dots
            List<Dot> newDots = new List<Dot>();
            for (int i = 0; i < numberOfDots; i++)
            {
                Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-randomOffsetRange.x, randomOffsetRange.x),
                    UnityEngine.Random.Range(-randomOffsetRange.y, randomOffsetRange.y),
                    UnityEngine.Random.Range(-randomOffsetRange.z, randomOffsetRange.z));
                Vector3 dotPosition = massCenterLocation + randomOffset;
                var dotColor = UnityEngine.Random.ColorHSV();

                GameObject dotObj = Instantiate(dotPrefab, dotPosition, Quaternion.identity);
                Dot dot = dotObj.GetComponent<Dot>();
                dot.Initialize(dotColor, dotPosition, radius);
                newDots.Add(dot);
            }


            // Add the new dots to the existing dots list
            int startIndex = _appStateFactory.State.Data.Dots.Count;
            foreach (var dot in newDots)
            {
                _appStateFactory.State.Data.Dots[startIndex] = dot;
                startIndex++;
            }

            // Return the list of newly created dots
            return newDots;
        }



    }
}