using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMobController : MobController
{
    protected override void Awake()
    {
        base.Awake();

        var enemyMaterial = Manager.Instance.Resource.Load<Material>(Define.RESOURCE_MATERIAL_ENEMY);
        _renderer.sharedMaterial = enemyMaterial;

        gameObject.tag = Define.TAG_ENEMY_MOB;
    }

    protected override void OnEnable()
    {
        _health = 10f;
        _collider.isTrigger = false;
    }

    protected override void OnDisable()
    {
        Manager.Instance.Object.ReturnObject(EObjectTypes.EnemyMob, gameObject);
    }

    protected override void _MoveMob()
    {
        _rigidbody.velocity = Vector3.zero;
        transform.localPosition += new Vector3(0f, 0f, _moveSpeed * transform.forward.z * Time.fixedDeltaTime);
    }

    protected override void _AttackTarget(Collision collision)
    {
        if (null != _targetMob)
            return;

        if (collision.gameObject.CompareTag(Define.TAG_ALLY_MOB))
        {
            _targetMob = collision.gameObject.GetComponent<IMobController>();
            _AttackTargetMob();
        }
    }
}
