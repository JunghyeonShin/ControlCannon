using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMobController : MobController
{
    private ICastleController _targetCastle;

    protected override void Awake()
    {
        base.Awake();

        gameObject.tag = Define.TAG_MOB;
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

    protected override void _FindTarget(Collision collision)
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

    private void _AttackTargetCastle()
    {
        _targetCastle.OnDamage();
        gameObject.SetActive(false);
    }
}
