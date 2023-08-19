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
    private GameObject _cannon;
    private Dictionary<EObjectTypes, ObjectPool> _objectsDic;

    private const int CREATE_MOB_COUNT = 150;

    public void Init()
    {
        var cannonParent = new GameObject(Define.ROOT_CANNON);
        _cannon = Manager.Instance.Resource.Instantiate(Define.RESOURCE_CANNON, cannonParent);

        _objectsDic = new Dictionary<EObjectTypes, ObjectPool>();
        for (EObjectTypes ii = 0; ii < EObjectTypes.COUNT; ++ii)
            _objectsDic.Add(ii, new ObjectPool());

        var objectsParent = new GameObject(Define.OBJECT_POOL);
        var mob = Manager.Instance.Resource.Load<GameObject>(Define.RESOURCE_MOB);
        _objectsDic[EObjectTypes.Mob].InitPool(mob, objectsParent, CREATE_MOB_COUNT);
        _objectsDic[EObjectTypes.EnemyMob].InitPool(mob, objectsParent, CREATE_MOB_COUNT);
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
