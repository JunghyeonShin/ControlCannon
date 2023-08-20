using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ECannonStates
{
    None,
    Ready,
    Shoot
}

public interface ICannonController
{
    public ECannonStates CannonState { get; set; }
    public void InitPosition();
}

public class CannonController : MonoBehaviour, ICannonController
{
    [SerializeField] private Transform _cannonLowerBody;
    [SerializeField] private float _arrivalTime = 2f;
    [SerializeField] private float _rotateTime = 1f;

    [SerializeField] private float _moveSpeed = 2.5f;
    [SerializeField] private float _createDelayTime = 2f;

    private DragHandler _dragHandler;
    private PointerHandler _selectHandler;

    private Coroutine _readyToPlayModeCoroutine;
    private Coroutine _createMobCoroutine;
    private bool _createMob;

    public ECannonStates CannonState { get; set; }

    private const float MAX_HORIZONTAL_MOVE_VALUE = 7f;
    private const float ADJUST_MOB_SPAWN_POINT = 3f;

    private readonly Vector3 INIT_SPAWN_POSITION = new Vector3(0f, 0f, -15f);
    private readonly Vector3 INIT_LOWER_BODY_ANGLE = new Vector3(0f, 270f, 0f);
    private readonly Vector3 GOAL_LOWER_BODY_ANGLE = new Vector3(0f, 360f, 0f);

    private void Awake()
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

    private void OnEnable()
    {
        CannonState = ECannonStates.Ready;
        if (null != _readyToPlayModeCoroutine)
        {
            StopCoroutine(_readyToPlayModeCoroutine);
            _readyToPlayModeCoroutine = null;
        }
        _readyToPlayModeCoroutine = StartCoroutine(_ReadyToPlayMode());
    }

    public void InitPosition()
    {
        transform.localPosition = Vector3.zero;
    }

    private IEnumerator _ReadyToPlayMode()
    {
        yield return null;
        transform.localPosition = INIT_SPAWN_POSITION;
        _cannonLowerBody.transform.localEulerAngles = INIT_LOWER_BODY_ANGLE;

        var titleUI = Manager.Instance.UI.GetUI<UI_Title>(Define.RESOURCE_UI_TITLE);
        while (null == titleUI)
        {
            titleUI = Manager.Instance.UI.GetUI<UI_Title>(Define.RESOURCE_UI_TITLE);
            yield return null;
        }
        titleUI.ActivePlayButton(false);

        var time = 0f;
        while (time <= _arrivalTime)
        {
            time += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(INIT_SPAWN_POSITION, Vector3.zero, time / _arrivalTime);
            yield return null;
        }
        transform.localPosition = Vector3.zero;

        time = 0f;
        while (time <= _rotateTime)
        {
            time += Time.deltaTime;
            _cannonLowerBody.localEulerAngles = Vector3.Lerp(INIT_LOWER_BODY_ANGLE, GOAL_LOWER_BODY_ANGLE, time / _rotateTime);
            yield return null;
        }
        _cannonLowerBody.localEulerAngles = Vector3.zero;

        titleUI.ActivePlayButton(true);
        _readyToPlayModeCoroutine = null;
    }

    private void _MoveHorizontal(PointerEventData eventData)
    {
        if (ECannonStates.Shoot != CannonState)
            return;

        var x = eventData.delta.x * _moveSpeed * Time.deltaTime;
        x = Mathf.Clamp(transform.localPosition.x + x, -MAX_HORIZONTAL_MOVE_VALUE, MAX_HORIZONTAL_MOVE_VALUE);
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }

    private void _StartCreateMob()
    {
        if (ECannonStates.Shoot != CannonState)
            return;

        _createMob = true;

        if (null == _createMobCoroutine)
            _createMobCoroutine = StartCoroutine(_CreateMob());
    }

    private IEnumerator _CreateMob()
    {
        while (_createMob)
        {
            if (ECannonStates.Ready == CannonState)
            {
                _createMob = false;
                break;
            }

            var allyMob = Manager.Instance.Object.GetObject(EObjectTypes.Mob);
            allyMob.transform.localPosition = transform.localPosition + transform.forward * ADJUST_MOB_SPAWN_POINT;
            var allyMobController = Utils.GetOrAddComponent<AllyMobController>(allyMob);
            allyMob.SetActive(true);
            allyMobController.InitGate();
            yield return YieldInstructionContainer.GetWaitForSeconds(_createDelayTime);
        }

        _createMobCoroutine = null;
    }

    private void _StopCreatingMob()
    {
        if (ECannonStates.Shoot != CannonState)
            return;

        _createMob = false;
    }
}
