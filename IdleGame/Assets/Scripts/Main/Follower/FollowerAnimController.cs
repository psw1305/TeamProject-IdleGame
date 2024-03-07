using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerAnimController : MonoBehaviour
{
    [SerializeField] GameObject charSprite;
    private Animator _animator;
    private int _isWalkBool;
    private int _isRangeTrigger;

    private void Awake()
    {
        _animator = charSprite.GetComponent<Animator>();
        _isWalkBool = Animator.StringToHash("isWalk");
        _isRangeTrigger = Animator.StringToHash("isRangeAtk");
    }

    public void OnWalk()
    {
        _animator.SetBool(_isWalkBool, true);
    }

    public void OnIdle()
    {
        _animator.SetBool(_isWalkBool, false);
    }

    public void OnRangeAtk()
    {
        _animator.SetTrigger(_isRangeTrigger);
    }
}
