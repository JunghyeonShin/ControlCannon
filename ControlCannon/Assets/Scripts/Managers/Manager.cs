using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private DataManager _data;
    private ObjectManager _object;
    private ResourceManager _resource;
    private StageManager _stage;
    private UIManager _uI;

    public DataManager Data { get { return _data; } }
    public ObjectManager Object { get { return _object; } }
    public ResourceManager Resource { get { return _resource; } }
    public StageManager Stage { get { return _stage; } }
    public UIManager UI { get { return _uI; } }

    private void Awake()
    {
        CreateInstance();

        _data = new DataManager();
        _resource = new ResourceManager();
        _stage = new StageManager();
        _uI = new UIManager();
        _object = new ObjectManager();

        _data.Init();
        _stage.Init();
        _uI.Init();
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
