using System.Collections;
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

    protected virtual void ApplySkillEffect()
    {
        //스킬 효과 적용
    }

    protected virtual void RemoveSkillEffect()
    {
        //스킬 효과 초기화
    }

    public void UseSkill()
    {
        if (!_canUse)
        {
            return;
        }
        _canUse = false;
        StartCoroutine(CountDurateTime());
    }

    private IEnumerator CountDurateTime()
    {
        if (_skillDurateTimeCoroutine == null)
        {
            gameObject.GetComponent<BaseSkill>().ApplySkillEffect();
            Debug.LogWarning("스킬 유지 시작");
            _currentDurateTime = effectDurateTime;
            while (_currentDurateTime >= 0)
            {
                yield return null;
                _currentDurateTime -= Time.deltaTime;
            }
            Debug.LogWarning("스킬 유지 종료");
            _skillDurateTimeCoroutine = null;

            StartCoroutine(CountSkillCooldown());
        }
    }


    private IEnumerator CountSkillCooldown()
    {
        if (_coolDownCoroutine == null)
        {
            gameObject.GetComponent<BaseSkill>().RemoveSkillEffect();
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
