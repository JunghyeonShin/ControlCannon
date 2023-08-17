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
    private Coroutine _createMobCoroutine;

    private bool _createMob;

    private const float MAX_HORIZONTAL_MOVE_VALUE = 7f;
    private const float ADJUST_MOB_SPAWN_POINT = 3f;

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
    }

    private void _MoveHorizontal(PointerEventData eventData)
    {
        var x = eventData.delta.x * _moveSpeed * Time.deltaTime;
        x = Mathf.Clamp(transform.localPosition.x + x, -MAX_HORIZONTAL_MOVE_VALUE, MAX_HORIZONTAL_MOVE_VALUE);
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }

    private void _StartCreateMob()
    {
        _createMob = true;

        if (null == _createMobCoroutine)
            _createMobCoroutine = StartCoroutine(_CreateMob());
    }

    private IEnumerator _CreateMob()
    {
        while (_createMob)
        {
            var mob = Manager.Instance.Object.GetObject(EObjectTypes.Mob);
            mob.transform.localPosition = transform.localPosition + transform.forward * ADJUST_MOB_SPAWN_POINT;
            var mobController = Utils.GetOrAddComponent<MobController>(mob);
            mobController.SetTag(Define.TAG_MOB);
            mob.SetActive(true);
            yield return YieldInstructionContainer.GetWaitForSeconds(_createDelayTime);
        }

        _createMobCoroutine = null;
    }

    private void _StopCreatingMob()
    {
        _createMob = false;
    }
}
