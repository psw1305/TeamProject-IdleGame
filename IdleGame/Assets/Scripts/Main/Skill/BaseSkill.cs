using System.Collections;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    private float _currentDurate;
    private float _currentCoolDown;

    private bool _canUse = true;
    [SerializeField] private float _effectDurate;
    [SerializeField] private float _coolDown;

    Coroutine _coolDownCoroutine;

    protected void UseSkill(ISkillUsingEffect usingEffect)
    {
        if (!_canUse)
        {
            return;
        }
        usingEffect.UsingSkillEffect();
        _canUse = false;
        StartCoroutine(CountCoolRoutine(_coolDown));
    }

    protected void StartCoolDown(float coolDown)
    {
        _coolDownCoroutine = StartCoroutine(CountCoolRoutine(coolDown));
    }

    private IEnumerator CountCoolRoutine(float coolDown)
    {
        if (_coolDownCoroutine == null)
        {
            _currentCoolDown = coolDown;
            while (_currentCoolDown >= 0)
            {
                yield return null;
                _currentCoolDown -= Time.deltaTime;
            }
            _canUse = true;
            _coolDownCoroutine = null;
        }
    }
}
