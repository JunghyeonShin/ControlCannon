using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectTypes
{
    Mob,
    EnemyMob,

    COUNT,
    None = COUNT
}

public class ObjectManager
{
    private Dictionary<EObjectTypes, ObjectPool> _objectsDic;

    private const string RESOURCE_MOB = "Objects/Mob";
    private const string OBJECT_POOL = "[OBJECT_POOL]";
    private const int CREATE_MOB_COUNT = 150;

    public void Init()
    {
        _objectsDic = new Dictionary<EObjectTypes, ObjectPool>();

        for (EObjectTypes ii = 0; ii < EObjectTypes.COUNT; ++ii)
            _objectsDic.Add(ii, new ObjectPool());

        #region TEMP
        var mob = Manager.Instance.Resource.Load<GameObject>(RESOURCE_MOB);
        var parent = new GameObject(OBJECT_POOL);
        _objectsDic[EObjectTypes.Mob].InitPool(mob, parent, CREATE_MOB_COUNT);
        _objectsDic[EObjectTypes.EnemyMob].InitPool(mob, parent, CREATE_MOB_COUNT);
        #endregion
    }

    public GameObject GetObject(EObjectTypes type)
    {
        return _objectsDic[type].GetObject();
    }

    public void ReturnObject(EObjectTypes type, GameObject go)
    {
        Utils.InitTransform(go);
        _objectsDic[type].ReturnObject(go);
    }
}
