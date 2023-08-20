using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectTypes
{
    Mob,
    EnemyMob,
    Gate,

    COUNT,
    None = COUNT
}

public class ObjectManager
{
    private GameObject _cannon;
    private ICannonController _currentCannon;
    private GameObject _castle;
    private Dictionary<EObjectTypes, ObjectPool> _objectsDic;

    private Queue<GameObject> _usedObjectsQueue;

    public ICannonController CurrentCannon { get { return _currentCannon; } }
    public GameObject Castle { get { return _castle; } }

    private const int CREATE_MOB_COUNT = 150;
    private const int CREATE_GATE_COUNT = 10;

    public void Init()
    {
        var cannonParent = new GameObject(Define.ROOT_CANNON);
        _cannon = Manager.Instance.Resource.Instantiate(Define.RESOURCE_OBJECT_CANNON, cannonParent);
        _currentCannon = _cannon.GetComponent<CannonController>();

        var castleParent = new GameObject(Define.ROOT_CASTLE);
        _castle = Manager.Instance.Resource.Instantiate(Define.RESOURCE_OBJECT_CASTLE, castleParent);
        _castle.SetActive(false);

        _objectsDic = new Dictionary<EObjectTypes, ObjectPool>();
        for (EObjectTypes ii = 0; ii < EObjectTypes.COUNT; ++ii)
            _objectsDic.Add(ii, new ObjectPool());

        var objectsParent = new GameObject(Define.OBJECT_POOL);
        var mob = Manager.Instance.Resource.Load<GameObject>(Define.RESOURCE_OBJECT_MOB);
        _objectsDic[EObjectTypes.Mob].InitPool(mob, objectsParent, CREATE_MOB_COUNT);
        _objectsDic[EObjectTypes.EnemyMob].InitPool(mob, objectsParent, CREATE_MOB_COUNT);
        var gate = Manager.Instance.Resource.Load<GameObject>(Define.RESOURCE_OBJECT_GATE);
        _objectsDic[EObjectTypes.Gate].InitPool(gate, objectsParent, CREATE_GATE_COUNT);

        _usedObjectsQueue = new Queue<GameObject>();
    }

    public GameObject GetObject(EObjectTypes type)
    {
        var obj = _objectsDic[type].GetObject();
        _usedObjectsQueue.Enqueue(obj);
        return obj;
    }

    public void ReturnUsedAllObject()
    {
        while (_usedObjectsQueue.Count > 0)
        {
            var usedObject = _usedObjectsQueue.Dequeue();
            if (usedObject.activeSelf)
                usedObject.SetActive(false);
        }
    }

    public void ReturnObject(EObjectTypes type, GameObject go)
    {
        Utils.InitTransform(go);
        _objectsDic[type].ReturnObject(go);
    }
}
