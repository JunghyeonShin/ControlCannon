using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager _instance;

    public static Manager Instance
    {
        get
        {
            if (null == _instance)
                _instance = GameObject.FindObjectOfType<Manager>();
            if (null == _instance)
            {
                var go = new GameObject(MANAGER);
                _instance = go.AddComponent<Manager>();
            }
            return _instance;
        }
    }

    private ObjectManager _object;

    public ObjectManager Object { get { return _object; } }

    private const string MANAGER = "[MANAGER]";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _object = new ObjectManager();
        _object.Init();
    }
}
