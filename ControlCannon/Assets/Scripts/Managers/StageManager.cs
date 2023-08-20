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

    private const string STAGE_STORAGE_PATH = "/Resources/Stages";
    private const string ANY_JSON_FILE = "*.json";
    private const string JSON_FILE = ".json";
    private const int NON_STAGE_INDEX = -1;

    public void Init()
    {
        _stagesList = new List<Stage>();

        var directoryPath = Application.dataPath + STAGE_STORAGE_PATH;
        if (Directory.Exists(directoryPath))
        {
            var directoryInfo = new DirectoryInfo(directoryPath);
            var filesCount = directoryInfo.GetFiles(ANY_JSON_FILE).Length;
            for (int ii = 0; ii < filesCount; ++ii)
            {
                var filePath = new StringBuilder();
                if (ii < 10)
                    filePath.Append(directoryPath + "/Stage_0" + ii + JSON_FILE);
                else
                    filePath.Append(directoryPath + "/Stage_" + ii + JSON_FILE);
                using (var fileStream = new FileStream(filePath.ToString(), FileMode.Open))
                {
                    var data = new byte[fileStream.Length];
                    fileStream.Read(data, 0, data.Length);
                    var json = Encoding.UTF8.GetString(data);
                    var stage = JsonUtility.FromJson<Stage>(json);
                    _stagesList.Add(stage);
                }
            }
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
