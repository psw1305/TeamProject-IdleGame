using System.Collections;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    protected float _currentDurateTime;
    protected float _currentCoolDown;
    protected SkillType _skillType;
    private bool _canUse;

    [SerializeField] private float effectDurateTime;
    [SerializeField] private float coolDown;

    Coroutine _skillDurateTimeCoroutine;
    Coroutine _coolDownCoroutine;

    protected virtual void Start()
    {
        _canUse = false;
        StartCoroutine(CountSkillCooldown());
    }

    protected abstract void ApplySkillEffect();

    protected abstract void RemoveSkillEffect();

    public void UseSkill()
    {
        if (!_canUse)
        {
            return;
        }

        if(_skillType == SkillType.Targeting && Manager.Game.Player.enemyList.Count == 0)
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
            _currentDurateTime = effectDurateTime;
            while (_currentDurateTime >= 0)
            {
                yield return null;
                _currentDurateTime -= Time.deltaTime;
            }
            _skillDurateTimeCoroutine = null;

            gameObject.GetComponent<BaseSkill>().RemoveSkillEffect();
            StartCoroutine(CountSkillCooldown());
        }
    }


    private IEnumerator CountSkillCooldown()
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
