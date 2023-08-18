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
}
