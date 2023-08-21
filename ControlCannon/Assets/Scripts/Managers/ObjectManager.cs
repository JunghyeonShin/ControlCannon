using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum EObjectTypes
{
    AllyMob,
    EnemyMob,
    Gate,

    COUNT,
    None = COUNT
}

public class ObjectManager
{
    private struct UsedObjects
    {
        public EObjectTypes ObjectsType { get; set; }
        public GameObject Object { get; set; }
    }

    private struct UsedObstacleObjects
    {
        public string ObstacleName { get; set; }
        public GameObject Object { get; set; }
    }

    private ICannonController _currentCannon;
    private GameObject _castle;

    private Dictionary<EObjectTypes, ObjectPool> _objectsDic;
    private Queue<UsedObjects> _usedObjectsQueue;

    private Dictionary<string, ObjectPool> _obstacleObjectsDic;
    private Queue<UsedObstacleObjects> _usedObstacleObjectsQueue;

    public ICannonController CurrentCannon { get { return _currentCannon; } }
    public GameObject Castle { get { return _castle; } }


    private const string DIGIT_OBSTACLE = "Obstacle_0";
    private const string NON_DIGIT_OBSTACLE = "Obstacle_";
    private const int DIGIT_VALUE = 10;
    private const int CREATE_MOB_COUNT = 150;
    private const int CREATE_GATE_COUNT = 10;
    private const int CREATE_OBSTACLE_COUNT = 1;

    public void Init()
    {
        var cannonParent = new GameObject(Define.ROOT_CANNON);
        var cannon = Manager.Instance.Resource.Instantiate(Define.RESOURCE_OBJECT_CANNON, cannonParent);
        _currentCannon = cannon.GetComponent<CannonController>();

        var castleParent = new GameObject(Define.ROOT_CASTLE);
        _castle = Manager.Instance.Resource.Instantiate(Define.RESOURCE_OBJECT_CASTLE, castleParent);
        _castle.SetActive(false);

        var objectsParent = new GameObject(Define.OBJECT_POOL);
        _objectsDic = new Dictionary<EObjectTypes, ObjectPool>();
        for (EObjectTypes ii = 0; ii < EObjectTypes.COUNT; ++ii)
            _objectsDic.Add(ii, new ObjectPool());
        var mob = Manager.Instance.Resource.Load<GameObject>(Define.RESOURCE_OBJECT_MOB);
        _objectsDic[EObjectTypes.AllyMob].InitPool(mob, objectsParent, CREATE_MOB_COUNT);
        _objectsDic[EObjectTypes.EnemyMob].InitPool(mob, objectsParent, CREATE_MOB_COUNT);
        var gate = Manager.Instance.Resource.Load<GameObject>(Define.RESOURCE_OBJECT_GATE);
        _objectsDic[EObjectTypes.Gate].InitPool(gate, objectsParent, CREATE_GATE_COUNT);
        _usedObjectsQueue = new Queue<UsedObjects>();

        var obstacleObjectsParent = new GameObject(Define.ROOT_OBSTACLE);
        _obstacleObjectsDic = new Dictionary<string, ObjectPool>();
        var obstacleName = new StringBuilder();
        var obstacleIndex = 0;
        while (true)
        {
            obstacleName.Clear();
            if (obstacleIndex < DIGIT_VALUE)
                obstacleName.Append(DIGIT_OBSTACLE + obstacleIndex);
            else
                obstacleName.Append(NON_DIGIT_OBSTACLE + obstacleIndex);
            var obstacle = Manager.Instance.Resource.Load<GameObject>(Define.RESOURCE_OBJECT_OBSTACLE + obstacleName.ToString());
            if (null == obstacle)
                break;

            var obstacleObjectPool = new ObjectPool();
            obstacleObjectPool.InitPool(obstacle, obstacleObjectsParent, CREATE_OBSTACLE_COUNT);
            _obstacleObjectsDic.Add(obstacleName.ToString(), obstacleObjectPool);
            ++obstacleIndex;
        }
        _usedObstacleObjectsQueue = new Queue<UsedObstacleObjects>();
    }

    public GameObject GetObject(EObjectTypes type)
    {
        var obj = _objectsDic[type].GetObject();
        var usedObject = new UsedObjects() { ObjectsType = type, Object = obj };
        _usedObjectsQueue.Enqueue(usedObject);
        return obj;
    }

    public GameObject GetObstacleObject(string obstacleName)
    {
        var obj = _obstacleObjectsDic[obstacleName].GetObject();
        var usedObstacleObject = new UsedObstacleObjects() { ObstacleName = obstacleName, Object = obj };
        _usedObstacleObjectsQueue.Enqueue(usedObstacleObject);
        return obj;
    }

    public void ReturnUsedAllObject()
    {
        while (_usedObjectsQueue.Count > 0)
        {
            var usedObject = _usedObjectsQueue.Dequeue();
            if (usedObject.Object.activeSelf)
                usedObject.Object.SetActive(false);
        }

        while (_usedObstacleObjectsQueue.Count > 0)
        {
            var usedObstacleObject = _usedObstacleObjectsQueue.Dequeue();
            if (usedObstacleObject.Object.activeSelf)
                usedObstacleObject.Object.SetActive(false);
        }
    }

    public void ReturnObject(EObjectTypes type, GameObject go)
    {
        Utils.InitTransform(go);
        _objectsDic[type].ReturnObject(go);
    }

    public void ReturnObstacleObject(string obstacleName, GameObject go)
    {
        Utils.InitTransform(go);
        _obstacleObjectsDic[obstacleName].ReturnObject(go);
    }
}
