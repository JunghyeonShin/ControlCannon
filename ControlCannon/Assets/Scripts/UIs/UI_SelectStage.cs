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

    private List<UI_StageButton> _stageButtonUIs;

    private const int MAX_STAGE_BUTTON = 20;

    protected override void _Init()
    {
        _BindButton(typeof(EButtons));
        _BindGameObject(typeof(EGameObjects));

        _BindEvent(_GetButton((int)EButtons.BackButton).gameObject, _OnClickBackButton);

        _stageButtonUIs = new List<UI_StageButton>();
        for (int ii = 0; ii < MAX_STAGE_BUTTON; ++ii)
        {
            var stageButton = Manager.Instance.Resource.Instantiate(Define.RESOURCE_UI_STAGE_BUTTON, _GetGameObject((int)EGameObjects.StagesList));
            var stageButtonUI = Utils.GetOrAddComponent<UI_StageButton>(stageButton);
            stageButtonUI.SetStageIndex(ii);
            stageButtonUI.OpenClearStage();
            _stageButtonUIs.Add(stageButtonUI);
        }
    }

    public void OpenClearStage()
    {
        var stageIndex = Mathf.Clamp(Manager.Instance.Data.ClearStageIndex + 1, 0, _stageButtonUIs.Count - 1);
        for (int ii = 0; ii < stageIndex; ++ii)
            _stageButtonUIs[ii].OpenClearStage();
    }

    private void _OnClickBackButton()
    {
        Manager.Instance.UI.ShowUI<UI_Title>(Define.RESOURCE_UI_TITLE);
    }
}
