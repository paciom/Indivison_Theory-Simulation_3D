using System.Collections.Generic;
using Collisions;
using UnityEngine;

namespace Models
{
    public class Dot : MonoBehaviour
    {
        public Color Color ;
        public int Id ;
        public int? ConnectedDotId ;
        public List<LocationSwapRecord> LocationSwapHistory  = new List<LocationSwapRecord>();
        public List<Vector3> LocationHistory = new List<Vector3>();
        public Vector3 Position
        {
            get => transform.position;
            set
            {
                if (transform.position != value)
                {
                    LocationHistory.Add(value);
                }
                transform.position = value;

            }
        }
        public float Radius ;
        public float Speed ;
        public Vector3 Direction ;
        public Dot ConnectedDot ;
        public LineRenderer ConnectionLine ;
        public List<CollisionRecord> Collisions = new List<CollisionRecord>();
        private Renderer _renderer;

        public void Initialize(Color color, Vector3 position, float radius)
        {
            Color = color;
            Position = position;
            Radius = radius;
            Speed = 1;
            Direction = Random.onUnitSphere;
            ConnectedDot = null;

            _renderer = GetComponent<Renderer>();
            _renderer.material.color = color;

            // Initialize LineRenderer
            ConnectionLine = gameObject.AddComponent<LineRenderer>();
            ConnectionLine.material = new Material(Shader.Find("Standard"));
            ConnectionLine.startWidth = 0.1f;
            ConnectionLine.endWidth = 0.1f;
            ConnectionLine.enabled = false; // Hide it initially

            Time.fixedDeltaTime = Time.timeScale * 0.000002f; // Assuming a default fixed timestep of 0.02 seconds

        }
        public void SetLineLocations()
        {
            ConnectionLine.SetPosition(0, Position);
            if (ConnectedDot != null)
            {
                ConnectionLine.SetPosition(1, ConnectedDot.Position);

            }
        }
    }
}