using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ClearStage : UI_Base
{
    private enum EButtons
    {
        BackButton,
        RestartButton,
        NextButton
    }

    private enum ETexts
    {
        StageText
    }

    private int _currentStageIndex = -1;

    private const int NEXT_STAGE = 1;
    #region TEMP
    private const int MAX_STAGE = 20;
    #endregion

    protected override void _Init()
    {
        _BindButton(typeof(EButtons));
        _BindText(typeof(ETexts));

        _BindEvent(_GetButton((int)EButtons.BackButton).gameObject, _OnClickBackButton);
        _BindEvent(_GetButton((int)EButtons.RestartButton).gameObject, _OnClickRestartButton);
        _BindEvent(_GetButton((int)EButtons.NextButton).gameObject, _OnClickNextButton);
    }

    public void SetStageIndex(int stageIndex)
    {
        _currentStageIndex = stageIndex;
        _GetText((int)ETexts.StageText).text = $"Stage {_currentStageIndex}";
    }

    private void _OnClickBackButton()
    {
        Manager.Instance.UI.ShowUI<UI_SelectStage>(Define.RESOURCE_UI_SELECT_STAGE, (selectStage) =>
        {
            selectStage.OpenClearStage();
        });
        Manager.Instance.Object.CurrentCannon.InitPosition();
    }

    private void _OnClickRestartButton()
    {
        Manager.Instance.UI.ShowUI<UI_Ingame>(Define.RESOURCE_UI_INGAME);
        Manager.Instance.Stage.LoadStage(_currentStageIndex);
    }

    private void _OnClickNextButton()
    {
        Manager.Instance.UI.ShowUI<UI_Ingame>(Define.RESOURCE_UI_INGAME);
        if (MAX_STAGE == _currentStageIndex)
            Manager.Instance.Stage.LoadStage(_currentStageIndex);
        else
            Manager.Instance.Stage.LoadStage(_currentStageIndex + NEXT_STAGE);
    }
}
