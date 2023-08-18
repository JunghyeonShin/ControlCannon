using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private ResourceManager _resource;
    private ObjectManager _object;

    public ResourceManager Resource { get { return _resource; } }
    public ObjectManager Object { get { return _object; } }

    private void Awake()
    {
        CreateInstance();

        _resource = new ResourceManager();
        _object = new ObjectManager();

        _object.Init();
    }

    #region Instance
    private static Manager _instance;

    public static Manager Instance { get { return _instance; } }

    public static void CreateInstance()
    {
        if (null != _instance)
            return;

        if (null == _instance)
            _instance = GameObject.FindObjectOfType<Manager>();
        if (null == _instance)
        {
            var go = new GameObject(Define.MANAGER);
            _instance = go.AddComponent<Manager>();
        }
        DontDestroyOnLoad(_instance.gameObject);
    }
    #endregion
}
