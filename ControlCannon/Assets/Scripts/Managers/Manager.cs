using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private ObjectManager _object;

    public ObjectManager Object { get { return _object; } }

    private void Awake()
    {
        CreateInstance();

        _object = new ObjectManager();
        _object.Init();
    }

    #region Instance
    private static Manager _instance;

    public static Manager Instance { get { return _instance; } }

    public void CreateInstance()
    {
        if (null == _instance)
            _instance = GameObject.FindObjectOfType<Manager>();
        if (null == _instance)
        {
            var go = new GameObject(Define.MANAGER);
            _instance = go.AddComponent<Manager>();
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion
}
