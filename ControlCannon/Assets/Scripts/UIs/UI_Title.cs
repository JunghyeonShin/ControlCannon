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

    private void _OnClickPlayButton()
    {
        #region TEMP
        Debug.Log("Play");
        #endregion
    }
}
