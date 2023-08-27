using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class StageManager
{
    private List<StageData> _stagesList;

    private int _currentStageIndex = -1;

    public int LastStageIndex { get { return _stagesList.Count; } }
    public StageData CurrentStageInfo
    {
        get
        {
            if (NON_STAGE_INDEX == _currentStageIndex)
                return null;
            return _stagesList[_currentStageIndex];
        }
    }

    private const string RESOURCE_STAGES = "Stages/";
    private const string DIGIT_STAGE = "Stage_0";
    private const string NON_DIGIT_STAGE = "Stage_";
    private const int NON_STAGE_INDEX = -1;
    private const int DIGIT_VALUE = 10;

    public void Init()
    {
        _stagesList = new List<StageData>();

        var fileIndex = 0;
        var resource = Manager.Instance.Resource;
        var filePath = new StringBuilder();
        while (true)
        {
            filePath.Clear();
            if (fileIndex < DIGIT_VALUE)
                filePath.Append(RESOURCE_STAGES + DIGIT_STAGE + fileIndex);
            else
                filePath.Append(RESOURCE_STAGES + NON_DIGIT_STAGE + fileIndex);
            var stageFile = resource.Load<TextAsset>(filePath.ToString());
            if (null == stageFile)
                break;

            var stage = JsonUtility.FromJson<StageData>(stageFile.ToString());
            _stagesList.Add(stage);
            ++fileIndex;
        }
    }

    public void LoadStage(int stageIndex)
    {
        _currentStageIndex = stageIndex;
        Manager.Instance.Object.CurrentCannon.CannonState = ECannonStates.Shoot;
        Manager.Instance.Object.CurrentCannon.InitPosition();
        Manager.Instance.Object.Castle.SetActive(true);

        for (int ii = 0; ii < _stagesList[_currentStageIndex].Gates.Length; ++ii)
        {
            var gate = Manager.Instance.Object.GetObject(EObjectTypes.Gate);
            var gateController = Utils.GetOrAddComponent<GateController>(gate);
            gateController.LoadIndex = ii;
            gate.SetActive(true);
        }

        for (int ii = 0; ii < _stagesList[_currentStageIndex].Obstacles.Length; ++ii)
        {
            var obstacle = Manager.Instance.Object.GetObstacleObject(_stagesList[_currentStageIndex].Obstacles[ii].ObstacleName);
            var obstacleController = Utils.GetOrAddComponent<ObstacleController>(obstacle);
            obstacleController.LoadIndex = ii;
            obstacle.SetActive(true);
        }
    }

    public void ClearStage()
    {
        Manager.Instance.Object.ReturnUsedAllObject();
        Manager.Instance.Object.CurrentCannon.CannonState = ECannonStates.Ready;
        if (_currentStageIndex > Manager.Instance.Data.ClearStageIndex)
            Manager.Instance.Data.ClearStageIndex = _currentStageIndex;
        Manager.Instance.UI.ShowUI<UI_ClearStage>(Define.RESOURCE_UI_CLEARSTAGE, (clearStageUI) =>
        {
            clearStageUI.SetStageIndex(_currentStageIndex);
        });
    }

    public void DefeatStage()
    {
        Manager.Instance.Object.Castle.SetActive(false);
        Manager.Instance.Object.ReturnUsedAllObject();
        Manager.Instance.Object.CurrentCannon.CannonState = ECannonStates.Ready;
        Manager.Instance.UI.ShowUI<UI_DefeatStage>(Define.RESOURCE_UI_DEFEATSTAGE, (defeatStageUI) =>
        {
            defeatStageUI.SetStageIndex(_currentStageIndex);
        });
    }
}
