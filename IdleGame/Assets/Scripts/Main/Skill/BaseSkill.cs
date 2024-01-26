using System.Collections;
using System.Linq;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    protected float _currentDurateTime;
    protected float _currentCoolDown;

    private bool _canUse = true;
    [SerializeField] private float effectDurateTime;
    [SerializeField] private float coolDown;

    Coroutine _skillDurateTimeCoroutine;
    Coroutine _coolDownCoroutine;


    protected void UseSkill(ISkillUsingEffect usingEffect)
    {
        if (!_canUse)
        {
            return;
        }
        _canUse = false;
        StartCoroutine(CountDurateTime(usingEffect));
    }

    private IEnumerator CountDurateTime(ISkillUsingEffect usingEffect)
    {
        if (_skillDurateTimeCoroutine == null)
        {
            usingEffect.ApplySkillEffect();
            Debug.LogWarning("스킬 유지 시작");
            _currentDurateTime = effectDurateTime;
            while (_currentDurateTime >= 0)
            {
                yield return null;
                _currentDurateTime -= Time.deltaTime;
            }
            Debug.LogWarning("스킬 유지 종료");
            _skillDurateTimeCoroutine = null;

            StartCoroutine(CountSkillCooldown(usingEffect));
        }
    }


    private IEnumerator CountSkillCooldown(ISkillUsingEffect usingEffect)
    {
        if (_coolDownCoroutine == null)
        {
            usingEffect.RemoveSkillEffect();
            Debug.LogWarning("스킬 쿨타임 시작");
            _currentCoolDown = coolDown;
            while (_currentCoolDown >= 0)
            {
                yield return null;
                _currentCoolDown -= Time.deltaTime;
            }
            _canUse = true;
            _coolDownCoroutine = null;
            Debug.LogWarning("스킬 쿨타임 종료");
        }
    }
}
