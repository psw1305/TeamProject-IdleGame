using System;
using UnityEngine;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts
{
    /// <summary>
    /// The main character script.
    /// </summary>
    public class Character : Creature
    {
        public Animator Animator;
        public CharacterController CharacterController;
        
        public void SetState(AnimationState state)
        {
            foreach (var variable in new[] { "Idle", "Ready", "Walking", "Running", "Crawling", "Jumping", "Climbing", "Blocking", "Dead" })
            {
                Animator.SetBool(variable, false);
            }

            switch (state)
            {
                case AnimationState.Idle: Animator.SetBool("Idle", true); break;
                case AnimationState.Ready: Animator.SetBool("Ready", true); break;
                case AnimationState.Walking: Animator.SetBool("Walking", true); break;
                case AnimationState.Running: Animator.SetBool("Running", true); break;
                case AnimationState.Crawling: Animator.SetBool("Crawling", true); break;
                case AnimationState.Jumping: Animator.SetBool("Jumping", true); break;
                case AnimationState.Climbing: Animator.SetBool("Climbing", true); break;
                case AnimationState.Blocking: Animator.SetBool("Blocking", true); break;
                case AnimationState.Dead: Animator.SetBool("Dead", true); break;
                default: throw new NotSupportedException();
            }

            //Debug.Log("SetState: " + state);
        }

        public AnimationState GetState()
        {
            if (Animator.GetBool("Idle")) return AnimationState.Idle;
            if (Animator.GetBool("Ready")) return AnimationState.Ready;
            if (Animator.GetBool("Walking")) return AnimationState.Walking;
            if (Animator.GetBool("Running")) return AnimationState.Running;
            if (Animator.GetBool("Crawling")) return AnimationState.Crawling;
            if (Animator.GetBool("Jumping")) return AnimationState.Jumping;
            if (Animator.GetBool("Climbing")) return AnimationState.Climbing;
            if (Animator.GetBool("Blocking")) return AnimationState.Blocking;
            if (Animator.GetBool("Dead")) return AnimationState.Dead;

            return AnimationState.Ready;
        }
    }
}