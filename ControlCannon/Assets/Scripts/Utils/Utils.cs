using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (null == component)
            component = go.AddComponent<T>();
        return component;
    }

    public static T FindChild<T>(GameObject go, string name) where T : UnityEngine.Object
    {
        if (null == go)
            return null;

        foreach (T component in go.GetComponentsInChildren<T>(true))
        {
            if (component.name.Equals(name))
                return component;
        }
        return null;
    }

    public static GameObject FindChild(GameObject go, string name)
    {
        var transform = FindChild<Transform>(go, name);
        if (null != transform)
            return transform.gameObject;
        return null;
    }

    public static void InitTransform(GameObject go)
    {
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
    }
}
