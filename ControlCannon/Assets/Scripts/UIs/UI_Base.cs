using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _uIObjectsDic = new Dictionary<Type, UnityEngine.Object[]>();

    private void Awake()
    {
        _Init();
    }

    protected abstract void _Init();

    protected void _BindEvent(GameObject go, Action action)
    {
        var pointerHandler = Utils.GetOrAddComponent<PointerHandler>(go);
        pointerHandler.OnPointerDownHandler -= action;
        pointerHandler.OnPointerDownHandler += action;
    }

    protected void _BindButton(Type type)
    {
        _Bind<Button>(type);
    }

    protected void _BindGameObject(Type type)
    {
        _Bind<GameObject>(type);
    }

    protected void _BindText(Type type)
    {
        _Bind<TextMeshProUGUI>(type);
    }

    protected Button _GetButton(int index)
    {
        return _Get<Button>(index);
    }

    protected GameObject _GetGameObject(int index)
    {
        return _Get<GameObject>(index);
    }

    protected TextMeshProUGUI _GetText(int index)
    {
        return _Get<TextMeshProUGUI>(index);
    }

    private void _Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] uIObjects = new UnityEngine.Object[names.Length];
        for (int ii = 0; ii < uIObjects.Length; ++ii)
        {
            if (typeof(GameObject) == typeof(T))
                uIObjects[ii] = Utils.FindChild(gameObject, names[ii]);
            else
                uIObjects[ii] = Utils.FindChild<T>(gameObject, names[ii]);

            if (null == uIObjects[ii])
                Debug.LogError($"Failed to bind({names[ii]})");
        }
        _uIObjectsDic.Add(typeof(T), uIObjects);
    }

    private T _Get<T>(int index) where T : UnityEngine.Object
    {
        if (_uIObjectsDic.TryGetValue(typeof(T), out var uIObjects))
            return uIObjects[index] as T;
        return null;
    }
}
