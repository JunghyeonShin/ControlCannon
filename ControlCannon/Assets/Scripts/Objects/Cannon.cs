using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2.5f;
    [SerializeField] private float _createDelayTime = 2f;

    private DragHandler _dragHandler;
    private PointerHandler _selectHandler;

    private bool _createMob;

    private const float MAX_HORIZONTAL_MOVE_VALUE = 7f;
    private const float ADJUST_MOB_SPAWN_POINT = 3f;

    private ObjectPool _tempObjectPool;
    private const string RESOURCE_MOB = "Objects/Mob";
    private const string OBJECT_POOL = "[OBJECT_POOL]";
    private const int CREATE_MOB_COUNT = 150;

    private void Start()
    {
        _dragHandler = gameObject.AddComponent<DragHandler>();
        _dragHandler.OnDragHandler -= _MoveHorizontal;
        _dragHandler.OnDragHandler += _MoveHorizontal;

        _selectHandler = gameObject.AddComponent<PointerHandler>();
        _selectHandler.OnPointerDownHandler -= _StartCreateMob;
        _selectHandler.OnPointerDownHandler += _StartCreateMob;
        _selectHandler.OnPointerUpHandler -= _StopCreatingMob;
        _selectHandler.OnPointerUpHandler += _StopCreatingMob;

        var mob = Resources.Load<GameObject>(RESOURCE_MOB);
        var parent = new GameObject(OBJECT_POOL);

        _tempObjectPool = new ObjectPool();
        _tempObjectPool.InitPool(mob, parent, CREATE_MOB_COUNT);
    }

    private void _StartCreateMob()
    {
        _createMob = true;
        StartCoroutine(_CreateMob());
    }

    private IEnumerator _CreateMob()
    {
        while (_createMob)
        {
            var mob = _tempObjectPool.GetObject();
            mob.transform.localPosition = transform.localPosition + transform.forward * ADJUST_MOB_SPAWN_POINT;
            mob.SetActive(true);
            yield return new WaitForSeconds(_createDelayTime);
        }
    }

    private void _StopCreatingMob()
    {
        _createMob = false;
    }

    private void _MoveHorizontal(PointerEventData eventData)
    {
        var x = eventData.delta.x * _moveSpeed * Time.deltaTime;
        x = Mathf.Clamp(transform.localPosition.x + x, -MAX_HORIZONTAL_MOVE_VALUE, MAX_HORIZONTAL_MOVE_VALUE);
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }
}
