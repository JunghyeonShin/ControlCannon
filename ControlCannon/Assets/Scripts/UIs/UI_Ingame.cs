using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Ingame : UI_Base
{
    private enum EButtons
    {
        BackButton
    }

    protected override void _Init()
    {
        _BindButton(typeof(EButtons));

        _BindEvent(_GetButton((int)EButtons.BackButton).gameObject, _OnClickBackButton);
    }

    private void _OnClickBackButton()
    {
        Manager.Instance.UI.ShowUI<UI_SelectStage>(Define.RESOURCE_UI_SELECT_STAGE, (selectStage) =>
        {
            selectStage.OpenClearStage();
        });
        Manager.Instance.Object.Castle.SetActive(false);
        Manager.Instance.Object.ReturnUsedAllObject();
        Manager.Instance.Object.CurrentCannon.CannonState = ECannonStates.Ready;
        Manager.Instance.Object.CurrentCannon.InitPosition();
    }
}
