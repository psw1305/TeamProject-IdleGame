using System.Collections;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    protected float _currentDurateTime;
    public float CurrentDurateTime => _currentDurateTime;


    protected float _currentCoolDown;
    public float CurrentCoolDown => _currentCoolDown;


    protected SkillType _skillType;
    private bool _canUse;

    [Header("Skill Time Setter")]
    [SerializeField] private float effectDurateTime;
    public float EffectDurateTime => effectDurateTime;

    [SerializeField] private float coolDown;
    public float CoolDown => coolDown;

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
