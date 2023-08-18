using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    private Dictionary<string, Object> _resources = new Dictionary<string, Object>();

    public T Load<T>(string key) where T : Object
    {
        if (_resources.TryGetValue(key, out var obj))
            return obj as T;

        obj = Resources.Load<T>(key);
        return obj as T;
    }

    public GameObject Instantiate(string key, GameObject parent = null)
    {
        var prefab = Load<GameObject>(key);
        var go = GameObject.Instantiate(prefab);
        if (null != parent)
        {
            go.transform.SetParent(parent.transform);
            Utils.InitTransform(go);
        }
        return go;
    }
}
