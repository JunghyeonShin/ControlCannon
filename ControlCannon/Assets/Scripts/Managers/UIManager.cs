using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private Dictionary<string, UI_Base> _uIsDic;

    private GameObject _root;
    private UI_Base _currentUI;

    public void Init()
    {
        _uIsDic = new Dictionary<string, UI_Base>();

        _root = new GameObject(Define.ROOT_UI);
        Utils.InitTransform(_root);
    }

    public void ShowUI<T>(string path, Action<T> callback = null) where T : UI_Base
    {
        if (null != _currentUI)
            _CloseCurrentUI();

        if (_uIsDic.TryGetValue(path, out _currentUI))
        {
            _ShowCurrentUI();
            callback?.Invoke(_currentUI as T);
            return;
        }

        var uI = Manager.Instance.Resource.Instantiate(path, _root);
        _currentUI = uI.GetComponent<UI_Base>();
        _uIsDic.Add(path, _currentUI);
        _ShowCurrentUI();
        callback?.Invoke(_currentUI as T);
    }

    public T GetUI<T>(string path) where T : UI_Base
    {
        if (_uIsDic.TryGetValue(path, out var uI))
            return uI as T;
        return null;
    }

    private void _CloseCurrentUI()
    {
        _currentUI.gameObject.SetActive(false);
        _currentUI = null;
    }

    private void _ShowCurrentUI()
    {
        _currentUI.gameObject.SetActive(true);
    }
}
