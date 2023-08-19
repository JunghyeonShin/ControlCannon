using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMobController : MobController
{
    private bool[] _passGate;
    private ICastleController _targetCastle;

    #region TEMP
    private const int TOTAL_GATE_COUNT = 10;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        gameObject.tag = Define.TAG_MOB;
        #region TEMP
        _passGate = new bool[TOTAL_GATE_COUNT];
        #endregion
    }

    protected override void OnEnable()
    {
        _health = 10f;
        _collider.isTrigger = false;
    }

    protected override void OnDisable()
    {
        Manager.Instance.Object.ReturnObject(EObjectTypes.Mob, gameObject);
    }

    protected override void _MoveMob()
    {
        var moveVec = new Vector3(0f, 0f, _moveSpeed * transform.forward.z * Time.fixedDeltaTime);
        _rigidbody.velocity = moveVec;
    }

    protected override void _AttackTarget(Collision collision)
    {
        if (null != _targetCastle || null != _targetMob)
            return;

        if (collision.gameObject.CompareTag(Define.TAG_ENEMY_CASTLE))
        {
            _targetCastle = collision.gameObject.GetComponent<ICastleController>();
            _AttackTargetCastle();
        }
        else if (collision.gameObject.CompareTag(Define.TAG_ENEMY_MOB))
        {
            _targetMob = collision.gameObject.GetComponent<IMobController>();
            _AttackTargetMob();
        }
    }

    public void InitGate()
    {
        for (int ii = 0; ii < _passGate.Length; ++ii)
            _passGate[ii] = false;
    }

    public bool IsPassedGate(int index)
    {
        return _passGate[index];
    }

    public void PassGate(int index)
    {
        _passGate[index] = true;
    }

    private void _AttackTargetCastle()
    {
        _targetCastle.OnDamage();
        gameObject.SetActive(false);
    }
}
