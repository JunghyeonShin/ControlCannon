using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StageButton : UI_Base
{
    private enum EButtons
    {
        StageButton
    }

    private enum ETexts
    {
        StageText
    }

    private int _stageIndex = -1;

    protected override void _Init()
    {
        _BindButton(typeof(EButtons));
        _BindText(typeof(ETexts));

        _BindEvent(_GetButton((int)EButtons.StageButton).gameObject, _OnClickStageButton);
    }

    public void SetStageIndex(int stageIndex)
    {
        _stageIndex = stageIndex;
        _GetText((int)ETexts.StageText).text = $"{_stageIndex + 1}";
    }

    private void _OnClickStageButton()
    {
        #region TEMP
        Debug.Log($"OnStage : {_stageIndex + 1}");
        #endregion
    }
}
