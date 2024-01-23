using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    [SerializeField] GameObject charSprite;
    private Animator _animator;
    private int _isWalkBool;
    private int _isDeadTrigger;
    private int _isReviveTrigger;
    private int _isMeleeTrigger;
    private int _isRangeTrigger;
    private int _isHitTrigger;
    private void Awake()
    {
        _animator = charSprite.GetComponent<Animator>();
        _isWalkBool = Animator.StringToHash("isWalk");
        _isDeadTrigger = Animator.StringToHash("isDead");
        _isReviveTrigger = Animator.StringToHash("isRevive");
        _isMeleeTrigger = Animator.StringToHash("isMeleeAtk");
        _isRangeTrigger = Animator.StringToHash("isRangeAtk");
        _isHitTrigger = Animator.StringToHash("isHit");
    }
    public void OnWalk()
    {
        _animator.SetBool(_isWalkBool, true);
    }
    public void OnIdle()
    {
        _animator.SetBool(_isWalkBool, false);
    }
    public void OnMeleeAtk()
    {
        _animator.SetTrigger(_isMeleeTrigger);
    }
    public void OnRangeAtk()
    {
        _animator.SetTrigger(_isRangeTrigger);
    }
    public void OnHit()
    {
        _animator.SetTrigger(_isHitTrigger);
    }
    public void OnDead()
    {
        _animator.SetTrigger(_isDeadTrigger);
    }
    public void OnRevive()
    {
        _animator.SetTrigger(_isReviveTrigger);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            OnIdle();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnWalk();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnMeleeAtk();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnRangeAtk();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnDead();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnRevive();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnHit();
        }
    }
}
