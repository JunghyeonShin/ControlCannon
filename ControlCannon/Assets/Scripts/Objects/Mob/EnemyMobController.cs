using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMobController : MobController
{
    protected override void Awake()
    {
        base.Awake();

        gameObject.tag = Define.TAG_ENEMY_MOB;
    }

    protected override void OnEnable()
    {
        _health = 10f;
    }

    protected override void OnDisable()
    {
        Manager.Instance.Object.ReturnObject(EObjectTypes.EnemyMob, gameObject);
    }

    protected override void _MoveMob()
    {
        var moveVec = new Vector3(0f, 0f, _moveSpeed * transform.forward.z * Time.fixedDeltaTime);
        _rigidbody.velocity = moveVec;
    }

    protected override void _FindTargetMob(Collision collision)
    {
        if (null != _targetMob)
            return;

        if (collision.gameObject.CompareTag(Define.TAG_MOB))
        {
            _targetMob = collision.gameObject.GetComponent<IMobController>();
            _AttackTargetMob();
        }
    }
}
