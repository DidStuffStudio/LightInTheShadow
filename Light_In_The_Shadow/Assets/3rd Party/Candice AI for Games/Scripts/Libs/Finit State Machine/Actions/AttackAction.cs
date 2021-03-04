using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ViridaxGameStudios.AI
{
    public class AttackAction : FSMAction
    {
        Animator animator;
        string finishEvent;
        public AttackAction(FSMState owner, Character aiController, string finishEvent = null) : base(owner, aiController)
        {
            animator = aiController.Animator;
            this.finishEvent = finishEvent;
        }
        public override void OnEnter()
        {
            if (aiController.hasAnimations)
            {
                if (aiController.animationType == AnimationType.CodeBased)
                {
                    AnimatorClipInfo[] clipInfo = aiController.Animator.GetCurrentAnimatorClipInfo(0);

                    if (!aiController.attackAnimationName.Equals(clipInfo[0].clip.name))
                    {
                        aiController.Animator.Play(aiController.attackAnimationName);
                    }
                }
                else
                    animator.SetTrigger(aiController.attackTransitionParameter);
            }
            
        }

        public override void OnExit()
        {

        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        
    }
}

