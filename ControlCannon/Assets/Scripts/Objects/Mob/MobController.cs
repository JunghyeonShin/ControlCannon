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
    [SerializeField] protected float _attack = 10f;
    [SerializeField] protected float _health = 10f;
    [SerializeField] protected float _moveSpeed = 150f;

    protected Rigidbody _rigidbody;
    protected Collider _collider;
    protected IMobController _targetMob;

    private Coroutine _attackTargetMobCoroutine;
    private Coroutine _deadCoroutine;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    protected abstract void OnEnable();

    private void OnCollisionEnter(Collision collision)
    {
        _FindTarget(collision);
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
            _collider.isTrigger = true;
            if (null != _deadCoroutine)
            {
                StopCoroutine(_deadCoroutine);
                _deadCoroutine = null;
            }
            _deadCoroutine = StartCoroutine(_OnDead());

            OnDeadCallback?.Invoke();
        }
    }

    protected abstract void _MoveMob();

    protected abstract void _FindTarget(Collision collision);

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

    private IEnumerator _OnDead()
    {
        yield return null;

        gameObject.SetActive(false);
        _deadCoroutine = null;
    }
}
