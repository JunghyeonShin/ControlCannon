using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class StageManager
{
    private List<Stage> _stagesList;

    private int _currentStageIndex = -1;

    public int LastStageIndex { get { return _stagesList.Count; } }
    public Stage CurrentStageInfo
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
        _stagesList = new List<Stage>();

        var fileIndex = 0;
        var resource = Manager.Instance.Resource;
        while (true)
        {
            var filePath = new StringBuilder();
            if (fileIndex < DIGIT_VALUE)
                filePath.Append(RESOURCE_STAGES + DIGIT_STAGE + fileIndex);
            else
                filePath.Append(RESOURCE_STAGES + NON_DIGIT_STAGE + fileIndex);
            var stageFile = resource.Load<TextAsset>(filePath.ToString());
            if (null == stageFile)
                break;

            var stage = JsonUtility.FromJson<Stage>(stageFile.ToString());
            _stagesList.Add(stage);
            ++fileIndex;
        }
    }

    public void LoadStage(int stageIndex)
    {
        //_currentStageIndex = stageIndex;
        #region TEMP
        _currentStageIndex = 0;
        #endregion
        Manager.Instance.Object.CurrentCannon.CannonState = ECannonStates.Shoot;
        Manager.Instance.Object.CurrentCannon.InitPosition();
        Manager.Instance.Object.Castle.SetActive(true);
        for (int ii = 0; ii < _stagesList[_currentStageIndex].gates.Length; ++ii)
        {
            var gate = Manager.Instance.Object.GetObject(EObjectTypes.Gate);
            var gateController = Utils.GetOrAddComponent<GateController>(gate);
            gateController.LoadIndex = ii;
            gate.SetActive(true);
        }
    }

    public void ClearStage()
    {
        Manager.Instance.Object.ReturnUsedAllObject();
        Manager.Instance.Object.CurrentCannon.CannonState = ECannonStates.Ready;
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
