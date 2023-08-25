using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public int LoadIndex { get; set; }

    private void OnEnable()
    {
        var currentStageInfo = Manager.Instance.Stage.CurrentStageInfo;
        if (null == currentStageInfo)
            return;

        transform.localPosition = currentStageInfo.Obstacles[LoadIndex].ObstaclePosition;
        transform.localEulerAngles = currentStageInfo.Obstacles[LoadIndex].ObstacleRotation;
        transform.localScale = currentStageInfo.Obstacles[LoadIndex].ObstacleScale;
    }

    private void OnDisable()
    {
        Manager.Instance.Object.ReturnObstacleObject(Manager.Instance.Stage.CurrentStageInfo.Obstacles[LoadIndex].ObstacleName, gameObject);
    }
}
