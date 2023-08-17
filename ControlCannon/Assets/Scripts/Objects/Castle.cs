using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [SerializeField] private float _totalCreateDelayTime;
    [SerializeField] private float _individualCreateDelayTime = 0.1f;
    [SerializeField] private float _health;
    [SerializeField] private int _createEnemyMobCount = 5;

    private Coroutine _createMobCoroutine;

    public bool IsAlive { get { return _health > 0f; } }

    private void OnEnable()
    {
        _StopCreateMobCoroutine();
        _createMobCoroutine = StartCoroutine(_CreateMob());
    }

    private void OnDisable()
    {
        _StopCreateMobCoroutine();
    }

    private IEnumerator _CreateMob()
    {
        while (IsAlive)
        {
            yield return YieldInstructionContainer.GetWaitForSeconds(_totalCreateDelayTime);

            for (int ii = 0; ii < _createEnemyMobCount; ++ii)
            {
                var enemyMob = Manager.Instance.Object.GetObject(EObjectTypes.EnemyMob);
                enemyMob.transform.localPosition = transform.localPosition + transform.forward;
                enemyMob.transform.localRotation = transform.localRotation;
                Utils.GetOrAddComponent<EnemyMobController>(enemyMob);
                enemyMob.SetActive(true);
                yield return YieldInstructionContainer.GetWaitForSeconds(_individualCreateDelayTime);
            }
        }
    }

    private void _StopCreateMobCoroutine()
    {
        if (null != _createMobCoroutine)
        {
            StopCoroutine(_createMobCoroutine);
            _createMobCoroutine = null;
        }
    }
}
