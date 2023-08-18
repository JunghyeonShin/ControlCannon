using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    private void Awake()
    {
        Manager.CreateInstance();
    }

    private void Start()
    {
        Manager.Instance.UI.ShowUI<UI_Title>(Define.RESOURCE_UI_TITLE);
    }
}
