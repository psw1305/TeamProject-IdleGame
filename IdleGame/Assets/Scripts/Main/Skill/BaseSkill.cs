using System.Collections;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    protected float _currentDurateTime;
    public float CurrentDurateTime => _currentDurateTime;


    protected float _currentCoolDown;
    public float CurrentCoolDown => _currentCoolDown;

    protected float _skillDamageRatio;

    private bool _canUse;

    protected Player _player;

    [Header("Skill Time Setter")]
    [SerializeField] private float effectDurateTime;
    public float EffectDurateTime => effectDurateTime;

    [SerializeField] private float coolDown;
    public float CoolDown => coolDown;

    Coroutine _skillDurateTimeCoroutine;
    Coroutine _coolDownCoroutine;

    protected virtual void Start()
    {
        _player = Manager.Game.Player;
        _canUse = false;
        _coolDownCoroutine = StartCoroutine(CountSkillCooldown());
    }

    protected abstract void ApplySkillEffect();

    protected abstract void RemoveSkillEffect();

    public void ResetSkill()
    {
        RemoveSkillEffect();
        _currentDurateTime = 0;
        _currentCoolDown = 0;
        if (_coolDownCoroutine != null)
        {
            StopCoroutine(_coolDownCoroutine);
            _coolDownCoroutine = null;
        }
        if (_skillDurateTimeCoroutine != null)
        {
            StopCoroutine(_skillDurateTimeCoroutine);
            _skillDurateTimeCoroutine = null;
        }
        _canUse = true;
    }

    public void UseSkill()
    {
        if (!_canUse)
        {
            return;
        }

        if (Manager.Game.Player.enemyList.Count == 0)
        {
            return;
        }

        if (Manager.Game.Player.State != PlayerState.Battle)
        {
            return;
        }

        _canUse = false;
        _skillDurateTimeCoroutine = StartCoroutine(CountDurateTime());
    }
    protected float CalculateDamageRatio(string skillID)
    {
        return (Manager.SkillData.SkillDataDictionary[skillID].SkillDamage
            + (Manager.Data.SkillInvenDictionary[skillID].level - 1) + Manager.SkillData.SkillDataDictionary[skillID].ReinforceDamage)
            / 100;
    }

    private IEnumerator CountDurateTime()
    {
        if (_skillDurateTimeCoroutine == null)
        {
            ApplySkillEffect();
            _currentDurateTime = effectDurateTime;
            while (_currentDurateTime >= 0)
            {
                yield return null;
                _currentDurateTime -= Time.deltaTime;
            }
            _skillDurateTimeCoroutine = null;

            RemoveSkillEffect();
            _coolDownCoroutine = StartCoroutine(CountSkillCooldown());
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
