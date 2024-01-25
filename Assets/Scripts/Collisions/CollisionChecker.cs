using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using SceneOperations;
using UnityEngine;
using Zenject;

namespace Collisions
{
    public class CollisionChecker : ICollisionChecker
    {
        private readonly IAppStateFactory _appStateFactory;
        public Dictionary<int, Dot> UniqueDots { get; private set; }
        private readonly IDotConnector _dotConnector;

        [Inject]
        public CollisionChecker
        (
            IAppStateFactory appStateFactory, 
            IDotConnector dotConnector
        )
        {
            _appStateFactory = appStateFactory;
            _dotConnector = dotConnector;

            UniqueDots = new Dictionary<int, Dot>();
        }
        private AppState State => _appStateFactory.State;
        public void HandleCollisions()
        {
            var collidedDots = GetCollidedDotPairs(_appStateFactory.State.Data.Dots);
            foreach (var pair in collidedDots)
            {
                _dotConnector.Connect(pair.A, pair.B);
            }
        }

        public List<Collision> GetCollidedDotPairs(Dictionary<int, Dot> dots)
        {
            var collidedPairs = new Dictionary<int, List<Collision>>();
            var collisionDistance = _appStateFactory.State.Data.CollisionRadius * 2;

            var keys = dots.Keys.ToList();
            for (var i = 0; i < keys.Count; i++)
            {
                for (var j = i + 1; j < keys.Count; j++)
                {
                    var idA = keys[i];
                    var idB = keys[j];

                    dots.TryGetValue(idA, out var a);
                    dots.TryGetValue(idB, out var b);
                    var pA = a.Position;
                    var pB = b.Position;

                    if (Math.Abs(pA.x - pB.x) > collisionDistance || Math.Abs(pA.y - pB.y) > collisionDistance || Math.Abs(pA.z - pB.z) > collisionDistance)
                        continue;
                    var distance = Vector3.Distance(a.Position, b.Position);

                    if (distance < _appStateFactory.State.Data.CollisionRadius && !AreSameDirection(a.Direction, b.Direction))
                    {
                        if (a.Id.CompareTo(b.Id) < 0)
                            AddPair(collidedPairs, a, b, distance);
                        else
                            AddPair(collidedPairs, b, a, distance);
                    }
                }
            }

            List<Collision> collisions = GetClosestCollisions(collidedPairs);
            return collisions;
        }

        private List<Collision> GetClosestCollisions(Dictionary<int, List<Collision>> collidedPairs)
        {
            var collisions = new List<Collision>();
            UniqueDots = new Dictionary<int, Dot>();

            foreach (var pairs in collidedPairs.Values)
            {
                pairs.Sort((a, b) => a.Distance.CompareTo(b.Distance));
                foreach (var pair in pairs)
                {
                    if (!UniqueDots.ContainsKey(pair.A.Id) && !UniqueDots.ContainsKey(pair.B.Id))
                    {
                        AddCollisionHistory(pair.A, pair.B);
                        AddCollisionHistory(pair.B, pair.A);
                        collisions.Add(pair);
                        UniqueDots[pair.A.Id] = pair.A;
                        UniqueDots[pair.B.Id] = pair.B;
                    }
                }
            }
            return collisions;
        }

        private void AddCollisionHistory(Dot a, Dot b)
        {
            a.Collisions.Add(new CollisionRecord(b.Id, State.Data.Time, a.Position));
        }

        private void AddPair(Dictionary<int, List<Collision>> collidedPairs, Dot a, Dot b, double distance)
        {
            if (!collidedPairs.ContainsKey(a.Id))
            {
                collidedPairs[a.Id] = new List<Collision>();
            }

            var aConnected = SwapConnected(a) ?? a;
            var bConnected = SwapConnected(b) ?? b;
            if (!AreSameDirection(aConnected.Direction, bConnected.Direction))
            {
                collidedPairs[a.Id].Add(new Collision(aConnected, bConnected, distance));
            }
        }

        private bool AreSameDirection(Vector3 offset1, Vector3 offset2)
        {
            double dotProduct = offset1.x * offset2.x + offset1.y * offset2.y + offset1.z * offset2.z;
            double angleInRadians = Math.Acos(dotProduct);
            double angleInDegrees = angleInRadians * 180.0 / Math.PI;

            return Math.Abs(angleInDegrees) <= 0.01;
        }

        private Dot SwapConnected(Dot a)
        {
            var conn = a.ConnectedDotId;
            if (conn == null)
            {
                return a;
            }
            else
            {
                var other = GetConnected(a);
                SwapPosition(other, a);
                Disconnect(other);
                Disconnect(a);
                return other;
            }
        }

        private void SwapPosition(Dot other, Dot a)
        {
            (other.Position, a.Position) = (a.Position, other.Position);
            RecordPositionSwap(a, other);
            RecordPositionSwap(other, a);
        }

        private Dot GetConnected(Dot d)
        {
            var connectedDotId = d.ConnectedDotId;
            if (connectedDotId == null)
            {
                return null;
            }
            else
            {
                return State.Data.Dots[(int)connectedDotId];
            }
        }

        public Dot GetDot(int id)
        {
            if (!State.Data.Dots.TryGetValue(id, out var dot))
            {
                throw new Exception($"Dot id {id} not found");
            }
            return dot;
        }

        private void Disconnect(Dot d)
        {
            var connectedDotId = d.ConnectedDotId;
            if (connectedDotId != null)
            {
                var connectedDot = State.Data.Dots[((int)connectedDotId!)];
                connectedDot.ConnectedDotId = null;
                d.ConnectedDotId = null;
            }
        }

        private void RecordPositionSwap(Dot a, Dot other)
        {
            a.LocationSwapHistory.Add(new LocationSwapRecord(a.Position, other.Position, State.Data.Time));

            if (a.LocationSwapHistory.Count>1)
            {
                var lineCreator = new HistoryLineCreator();
                var swap0 = a.LocationSwapHistory.Last();
                var swap1 = a.LocationSwapHistory[a.LocationSwapHistory.Count - 2];
                var lineRenderer =  lineCreator.CreateNewLine(a.Id, swap0.End, swap1.Start, a.Color, State.Data.Time);
                State.Ui.HistoryLines.Add(lineRenderer);
            }

        }
    }
}
