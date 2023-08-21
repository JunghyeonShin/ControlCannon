using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAllyMobMoves
{
    None,
    Forward,
    Tracking,
}

public interface IAllyMobController
{
    public EAllyMobMoves AllyMoveTypes { get; set; }
}

public class AllyMobController : MobController, IAllyMobController
{
    private bool[] _passGate;
    private ICastleController _targetCastle;

    #region TEMP
    private const int TOTAL_GATE_COUNT = 10;
    #endregion

    public EAllyMobMoves AllyMoveTypes { get; set; }

    protected override void Awake()
    {
        base.Awake();

        var allyMaterial = Manager.Instance.Resource.Load<Material>(Define.RESOURCE_MATERIAL_ALLY);
        _renderer.sharedMaterial = allyMaterial;

        gameObject.tag = Define.TAG_ALLY_MOB;

        #region TEMP
        _passGate = new bool[TOTAL_GATE_COUNT];
        #endregion
    }

    protected override void OnEnable()
    {
        _health = 10f;
        _collider.isTrigger = false;
        AllyMoveTypes = EAllyMobMoves.Forward;
    }

    protected override void OnDisable()
    {
        AllyMoveTypes = EAllyMobMoves.None;
        Manager.Instance.Object.ReturnObject(EObjectTypes.Mob, gameObject);
    }

    protected override void _MoveMob()
    {
        _rigidbody.velocity = Vector3.zero;
        if (EAllyMobMoves.Forward == AllyMoveTypes)
            transform.localPosition += new Vector3(0f, 0f, _moveSpeed * transform.forward.z * Time.fixedDeltaTime);
        else if (EAllyMobMoves.Tracking == AllyMoveTypes)
        {
            var targetTransform = Manager.Instance.Object.Castle.transform;
            transform.forward = (targetTransform.localPosition - transform.position).normalized;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetTransform.localPosition, _moveSpeed * Time.fixedDeltaTime);
        }
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
        _targetCastle = null;
        gameObject.SetActive(false);
    }
}
