using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StageButton : UI_Base
{
    private enum EButtons
    {
        StageButton
    }

    private enum EGameObjects
    {
        LockPanel
    }

    private enum ETexts
    {
        StageText
    }

    private int _stageIndex = -1;

    private const int ADJUST_INDEX = 1;

    protected override void _Init()
    {
        _BindButton(typeof(EButtons));
        _BindGameObject(typeof(EGameObjects));
        _BindText(typeof(ETexts));

        _BindEvent(_GetButton((int)EButtons.StageButton).gameObject, _OnClickStageButton);
    }

    public void SetStageIndex(int stageIndex)
    {
        _stageIndex = stageIndex + ADJUST_INDEX;
        _GetText((int)ETexts.StageText).text = $"{_stageIndex}";
    }

    public void OpenClearStage()
    {
        var stageTextObject = _GetText((int)ETexts.StageText).gameObject;
        if (_stageIndex <= Manager.Instance.Data.ClearStageIndex + ADJUST_INDEX)
        {
            stageTextObject.SetActive(true);
            _GetGameObject((int)EGameObjects.LockPanel).SetActive(false);
        }
        else
        {
            stageTextObject.SetActive(false);
            _GetGameObject((int)EGameObjects.LockPanel).SetActive(true);
        }
    }

    private void _OnClickStageButton()
    {
        Manager.Instance.UI.ShowUI<UI_Ingame>(Define.RESOURCE_UI_INGAME);
        Manager.Instance.Stage.LoadStage(_stageIndex);
    }
}
