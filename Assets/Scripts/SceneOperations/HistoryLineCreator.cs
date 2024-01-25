using Models;
using UnityEngine;

namespace SceneOperations
{
    public class HistoryLineCreator : IHistoryLineCreator
    {
        public HistoryLine CreateNewLine(int dotId, Vector3 startPoint, Vector3 endPoint, Color color, int time)
        {
            // Create a new empty GameObject
            var lineGO = new GameObject("Path" + dotId);
            LineRenderer lineRenderer = lineGO.AddComponent<LineRenderer>();

            //// Generate a random color
            //Color randomColor = new Color(
            //    Random.Range(0f, 1f), // Red
            //    Random.Range(0f, 1f), // Green
            //    Random.Range(0f, 1f), // Blue
            //    1f // Alpha, fully opaque
            //);

            // Set up the LineRenderer
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"))
            {
                color = color
            }; // Use a built-in shader
            lineRenderer.startWidth = 0.1f; // Set the width of the line
            lineRenderer.endWidth = 0.1f;

            //// Set positions - this example uses random positions
            //Vector3 startPoint = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
            //Vector3 endPoint = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));

            // Set the number of vertex count
            lineRenderer.positionCount = 2;

            // Set the positions
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);

            return new HistoryLine()
            {
                LineRender = lineRenderer,
                DotId = dotId,
                Time = time,
            };
        }
    }
}