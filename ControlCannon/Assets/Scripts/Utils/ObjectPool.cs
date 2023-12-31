using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Queue<GameObject> _pool = new Queue<GameObject>();
    private GameObject _origin;
    private GameObject _parent;

    public void InitPool(GameObject prefab, GameObject parent, int count)
    {
        _origin = prefab;
        _parent = parent;

        for (int ii = 0; ii < count; ++ii)
            _pool.Enqueue(_CreateObject());
    }

    public GameObject GetObject()
    {
        if (_pool.Count > 0)
            return _pool.Dequeue();
        else
        {
            var newObject = _CreateObject();
            return newObject;
        }
    }

    public void ReturnObject(GameObject go)
    {
        Utils.InitTransform(go);
        _pool.Enqueue(go);
    }

    private GameObject _CreateObject()
    {
        var go = GameObject.Instantiate(_origin);
        go.transform.SetParent(_parent.transform);
        Utils.InitTransform(go);
        go.SetActive(false);
        return go;
    }
}
