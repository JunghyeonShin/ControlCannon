using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public int LoadIndex { get; set; }

    private void OnEnable()
    {
        transform.localPosition = Manager.Instance.Stage.CurrentStageInfo.Obstacles[LoadIndex].ObstaclePosition;
        transform.localEulerAngles = Manager.Instance.Stage.CurrentStageInfo.Obstacles[LoadIndex].ObstacleRotation;
    }

    private void OnDisable()
    {
        Manager.Instance.Object.ReturnObstacleObject(Manager.Instance.Stage.CurrentStageInfo.Obstacles[LoadIndex].ObstacleName, gameObject);
    }
}
