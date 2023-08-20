using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICastleController
{
    public void OnDamage();
}

public class CastleController : MonoBehaviour, ICastleController
{
    [SerializeField] private float _totalCreateDelayTime;
    [SerializeField] private float _individualCreateDelayTime = 0.1f;
    [SerializeField] private float _health;
    [SerializeField] private int _createEnemyMobCount = 5;

    private Coroutine _createMobCoroutine;

    private const float MIN_RANDOM_POSITION_X = -1f;
    private const float MAX_RANDOM_POSITION_X = 1f;
    private const float MIN_RANDOM_POSITION_Z = -1f;
    private const float MAX_RANDOM_POSITION_Z = 0f;
    private const int DAMAGED_VALUE = 1;

    private readonly Vector3 DEFAULT_CASTLE_SCALE = new Vector3(0.3f, 0.3f, 0.3f);

    private void OnEnable()
    {
        var currentStageInfo = Manager.Instance.Stage.CurrentStageInfo;
        if (null == currentStageInfo)
            return;

        transform.localPosition = Manager.Instance.Stage.CurrentStageInfo.castlePosition;
        transform.localEulerAngles = Manager.Instance.Stage.CurrentStageInfo.castleRotation;
        transform.localScale = DEFAULT_CASTLE_SCALE;

        _StopCreateMobCoroutine();
        _createMobCoroutine = StartCoroutine(_CreateMob());
    }

    private void OnDisable()
    {
        _StopCreateMobCoroutine();
    }

    public void OnDamage()
    {
        _health -= DAMAGED_VALUE;
        if (_health <= 0f)
            gameObject.SetActive(false);
    }

    private IEnumerator _CreateMob()
    {
        while (_health > 0f)
        {
            yield return YieldInstructionContainer.GetWaitForSeconds(_totalCreateDelayTime);

            for (int ii = 0; ii < _createEnemyMobCount; ++ii)
            {
                var enemyMob = Manager.Instance.Object.GetObject(EObjectTypes.EnemyMob);
                var randomPosition = new Vector3(Random.Range(MIN_RANDOM_POSITION_X, MAX_RANDOM_POSITION_X), 0f, Random.Range(MIN_RANDOM_POSITION_Z, MAX_RANDOM_POSITION_Z));
                enemyMob.transform.localPosition = transform.localPosition + transform.forward * 2f + randomPosition;
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
