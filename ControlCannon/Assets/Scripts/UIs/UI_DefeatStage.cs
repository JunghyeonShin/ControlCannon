using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DefeatStage : UI_Base
{
    private enum EButtons
    {
        BackButton,
        RestartButton
    }

    private enum ETexts
    {
        StageText
    }

    protected override void _Init()
    {
        _BindButton(typeof(EButtons));
        _BindText(typeof(ETexts));

        _BindEvent(_GetButton((int)EButtons.BackButton).gameObject, _OnClickBackButton);
        _BindEvent(_GetButton((int)EButtons.RestartButton).gameObject, _OnClickRestartButton);
    }

    private int _currentStageIndex = -1;

    public void SetStageIndex(int stageIndex)
    {
        _currentStageIndex = stageIndex;
        _GetText((int)ETexts.StageText).text = $"Stage {_currentStageIndex}";
    }

    private void _OnClickBackButton()
    {
        Manager.Instance.UI.ShowUI<UI_SelectStage>(Define.RESOURCE_UI_SELECT_STAGE);
        Manager.Instance.Object.CurrentCannon.InitPosition();
    }

    private void _OnClickRestartButton()
    {
        Manager.Instance.UI.ShowUI<UI_Ingame>(Define.RESOURCE_UI_INGAME);
        Manager.Instance.Stage.LoadStage(_currentStageIndex);
    }
}
