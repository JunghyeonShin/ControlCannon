using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2.5f;

    private DragHandler _dragHandler;

    private const float MAX_HORIZONTAL_MOVE_VALUE = 7f;

    private void Start()
    {
        _dragHandler = gameObject.AddComponent<DragHandler>();
        _dragHandler.OnDragHandler -= _MoveHorizontal;
        _dragHandler.OnDragHandler += _MoveHorizontal;
    }

    private void _MoveHorizontal(PointerEventData eventData)
    {
        var x = eventData.delta.x * _moveSpeed * Time.deltaTime;
        x = Mathf.Clamp(transform.localPosition.x + x, -MAX_HORIZONTAL_MOVE_VALUE, MAX_HORIZONTAL_MOVE_VALUE);
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }
}
