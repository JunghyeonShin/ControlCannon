using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SelectStage : UI_Base
{
    private enum EButtons
    {
        BackButton
    }

    private enum EGameObjects
    {
        StagesList
    }

    private const int MAX_STAGE_BUTTON = 20;

    protected override void _Init()
    {
        _BindButton(typeof(EButtons));
        _BindGameObject(typeof(EGameObjects));

        _BindEvent(_GetButton((int)EButtons.BackButton).gameObject, _OnClickBackButton);
        for (int ii = 0; ii < MAX_STAGE_BUTTON; ++ii)
        {
            var stageButton = Manager.Instance.Resource.Instantiate(Define.RESOURCE_UI_STAGE_BUTTON, _GetGameObject((int)EGameObjects.StagesList));
            var stageButtonUI = Utils.GetOrAddComponent<UI_StageButton>(stageButton);
            stageButtonUI.SetStageIndex(ii);
        }
    }

    private void _OnClickBackButton()
    {
        Manager.Instance.UI.ShowUI<UI_Title>(Define.RESOURCE_UI_TITLE);
    }
}
