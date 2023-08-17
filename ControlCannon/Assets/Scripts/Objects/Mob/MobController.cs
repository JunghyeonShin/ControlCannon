using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMobController
{
    public void OnDamage(float damage, Action OnDeadCallback);
}

public abstract class MobController : MonoBehaviour, IMobController
{
    [SerializeField] protected float _attackDelayTime = 1f;
    [SerializeField] protected float _attack = 5f;
    [SerializeField] protected float _health = 10f;
    [SerializeField] protected float _moveSpeed = 150f;

    protected Rigidbody _rigidbody;
    protected IMobController _targetMob;

    private Coroutine _attackTargetMobCoroutine;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected abstract void OnEnable();

    private void OnCollisionEnter(Collision collision)
    {
        _FindTargetMob(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        _targetMob = null;
    }

    private void FixedUpdate()
    {
        _MoveMob();
    }

    protected abstract void OnDisable();

    public void OnDamage(float damage, Action OnDeadCallback)
    {
        _health -= damage;
        if (_health <= 0f)
        {
            gameObject.SetActive(false);
            OnDeadCallback?.Invoke();
        }
    }

    protected abstract void _MoveMob();

    protected abstract void _FindTargetMob(Collision collision);

    protected void _AttackTargetMob()
    {
        if (null != _attackTargetMobCoroutine)
        {
            StopCoroutine(_attackTargetMobCoroutine);
            _attackTargetMobCoroutine = null;
        }

        if (gameObject.activeSelf)
            _attackTargetMobCoroutine = StartCoroutine(_AttackTargetMobEnumerator());
    }

    private IEnumerator _AttackTargetMobEnumerator()
    {
        while (null != _targetMob)
        {
            _targetMob.OnDamage(_attack, _DieTargetMob);
            yield return YieldInstructionContainer.GetWaitForSeconds(_attackDelayTime);
        }
    }

    private void _DieTargetMob()
    {
        _targetMob = null;
    }
}
