using System.Collections.Generic;
using Models;
using UnityEngine;
using Zenject;

namespace SceneOperations
{
    public class SceneResetter : ISceneResetter
    {
        [Inject]
        public SceneResetter()
        {
        }

        public void EmptyScene(AppState state)
        {
            // Delete
            foreach (var dot in state.Data.Dots.Values)
            {
                Object.Destroy(dot.gameObject);
                //Destroy(dot);
            }

            state.Data.Dots = new Dictionary<int, Dot>();


            foreach (var line in state.Ui.HistoryLines)
            {
                Object.Destroy(line.LineRender.gameObject);
            }

            state.Ui.HistoryLines = new List<HistoryLine>();
            state.Data.Dots = new Dictionary<int, Dot>();
        }
    }
}