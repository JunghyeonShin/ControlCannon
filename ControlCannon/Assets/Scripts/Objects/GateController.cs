using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EGateTypes
{
    None,
    NonMovable,
    Movable,
}

public class GateController : MonoBehaviour
{
    private enum EMoveTypes
    {
        None,
        LeftMove,
        RightMove,
    }

    private TextMeshProUGUI _multiplierText;
    private int _multiplier;
    private EGateTypes _gateType;
    private EMoveTypes _moveType;
    private float _moveSpeed;

    public int LoadIndex { get; set; }

    private const string MULTIPLIER_OBJECT_NAME = "Multiplier";
    private const float MIN_RANDOM_POSITION_X = -1f;
    private const float MAX_RANDOM_POSITION_X = 1f;
    private const float MIN_RANDOM_POSITION_Z = 0f;
    private const float MAX_RANDOM_POSITION_Z = 1f;
    private const float MIN_MOVE_SPEED = 3f;
    private const float MAX_MOVE_SPEED = 5f;
    private const float CUT_LINE = 6.5f;
    private const int SELF = 1;
    private const int ADJUST_MOVE_TYPE = 1;
    private const int INVERSE_VALUE = -1;

    private void Awake()
    {
        var gateRenderer = GetComponentInChildren<Renderer>();
        var allyMaterial = Manager.Instance.Resource.Load<Material>(Define.RESOURCE_MATERIAL_ALLY);
        gateRenderer.sharedMaterial = allyMaterial;

        _multiplierText = Utils.FindChild<TextMeshProUGUI>(gameObject, MULTIPLIER_OBJECT_NAME);
    }

    private void OnEnable()
    {
        var currentStageInfo = Manager.Instance.Stage.CurrentStageInfo;
        if (null == currentStageInfo)
            return;

        transform.localPosition = currentStageInfo.Gates[LoadIndex].GatePosition;
        transform.localScale = currentStageInfo.Gates[LoadIndex].GateScale;
        _multiplier = currentStageInfo.Gates[LoadIndex].Multiplier;
        _multiplierText.text = $"X{_multiplier}";
        if (Enum.TryParse(currentStageInfo.Gates[LoadIndex].GateType, out _gateType))
        {
            if (EGateTypes.Movable == _gateType)
            {
                _moveType = (EMoveTypes)UnityEngine.Random.Range((int)EMoveTypes.LeftMove, (int)EMoveTypes.RightMove + ADJUST_MOVE_TYPE);
                _moveSpeed = UnityEngine.Random.Range(MIN_MOVE_SPEED, MAX_MOVE_SPEED);
            }
        }
    }

    private void FixedUpdate()
    {
        if (EGateTypes.Movable != _gateType)
            return;

        if (EMoveTypes.RightMove == _moveType && transform.localPosition.x >= CUT_LINE)
            _moveType = EMoveTypes.LeftMove;
        else if (EMoveTypes.LeftMove == _moveType && transform.localPosition.x <= -CUT_LINE)
            _moveType = EMoveTypes.RightMove;

        if (EMoveTypes.RightMove == _moveType)
            _moveSpeed = Mathf.Abs(_moveSpeed);
        else if (EMoveTypes.LeftMove == _moveType)
            _moveSpeed = INVERSE_VALUE * Mathf.Abs(_moveSpeed);
        transform.localPosition += new Vector3(_moveSpeed * Time.fixedDeltaTime, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.TAG_ALLY_MOB))
        {
            var allyMobController = other.gameObject.GetComponent<AllyMobController>();
            if (allyMobController.IsPassedGate(LoadIndex))
                return;

            allyMobController.PassGate(LoadIndex);
            for (int ii = 0; ii < _multiplier - SELF; ++ii)
            {
                var replicaAllyMob = Manager.Instance.Object.GetObject(EObjectTypes.AllyMob);
                var randomPosition = new Vector3(UnityEngine.Random.Range(MIN_RANDOM_POSITION_X, MAX_RANDOM_POSITION_X), 0f, UnityEngine.Random.Range(MIN_RANDOM_POSITION_Z, MAX_RANDOM_POSITION_Z));
                replicaAllyMob.transform.localPosition = other.transform.localPosition + randomPosition;
                var replicaAllyMobController = Utils.GetOrAddComponent<AllyMobController>(replicaAllyMob);
                replicaAllyMob.SetActive(true);
                replicaAllyMobController.PassGate(LoadIndex);
            }
        }
    }

    private void OnDisable()
    {
        Manager.Instance.Object.ReturnObject(EObjectTypes.Gate, gameObject);
    }
}
