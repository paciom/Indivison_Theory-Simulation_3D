using Models;
using UnityEngine;

namespace SceneOperations
{
    public interface IHistoryLineCreator
    {
        HistoryLine CreateNewLine(int dotId, Vector3 startPoint, Vector3 endPoint, Color color, int time);
    }
}