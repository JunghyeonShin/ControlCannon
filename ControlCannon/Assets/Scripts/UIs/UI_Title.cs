using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Title : UI_Base
{
    private enum EButtons
    {
        PlayButton
    }

    protected override void _Init()
    {
        _BindButton(typeof(EButtons));

        _BindEvent(_GetButton((int)EButtons.PlayButton).gameObject, _OnClickPlayButton);
    }

    public void ActivePlayButton(bool active)
    {
        _GetButton((int)EButtons.PlayButton).gameObject.SetActive(active);
    }

    private void _OnClickPlayButton()
    {
        Manager.Instance.UI.ShowUI<UI_SelectStage>(Define.RESOURCE_UI_SELECT_STAGE);
    }
}
