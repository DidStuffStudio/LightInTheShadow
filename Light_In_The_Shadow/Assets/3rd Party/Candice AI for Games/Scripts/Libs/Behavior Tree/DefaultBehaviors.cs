using System;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace ViridaxGameStudios.AI
{
    public class DefaultBehaviors
    {
        #region CHECK/VERIFY ACTION TASKS
        public static BehaviorStates EnemyDetected(BehaviorNode rootNode)
        {

            CandiceAIController agent = rootNode.aiController;
            if (agent._enemyFound)
            {
                //Set the main target as the enemy
                if (agent._enemies.Count > 0)
                {
                    rootNode.aiController.mainTarget = agent._enemies[0];
                }
                return BehaviorStates.SUCCESS;
            }
            else
                return BehaviorStates.FAILURE;
        }

       

        public static BehaviorStates AllyDetected(BehaviorNode rootNode)
        {
            CandiceAIController agent = rootNode.aiController;
            if (agent._allyFound)
            {
                //Set the main target as the enemy
                if (agent._allies.Count > 0)
                {
                    rootNode.aiController.mainTarget = agent._allies[0];
                }
                return BehaviorStates.SUCCESS;
            }
            else
                return BehaviorStates.FAILURE;
        }
        public static BehaviorStates PlayerDetected(BehaviorNode rootNode)
        {
            CandiceAIController agent = rootNode.aiController;
            if (agent._playerFound)
            {
                rootNode.aiController.mainTarget = agent.player;
                return BehaviorStates.SUCCESS;
            }

            else
                return BehaviorStates.FAILURE;
        }
        public static BehaviorStates WithinAttackRange(BehaviorNode rootNode)
        {
            float distance = float.MaxValue;
            CandiceAIController agent = rootNode.aiController;
            try
            {
                distance = Vector3.Distance(agent.transform.position, agent.attackTarget.transform.position);
            }
            catch (Exception e)
            {
                Debug.LogError("DefaultBehaviors.WithinAttackRange: No target within attack range: " + e.Message);
            }
            if (distance <= agent.statAttackRange.value)
                return BehaviorStates.SUCCESS;
            else
                return BehaviorStates.FAILURE;
        }
        public static BehaviorStates IsPatrolling(BehaviorNode rootNode)
        {
            CandiceAIController agent = rootNode.aiController;
            if (agent._isPatrolling)
                return BehaviorStates.SUCCESS;
            else
                return BehaviorStates.FAILURE;
        }

        public static BehaviorStates IsDead(BehaviorNode rootNode)
        {
            BehaviorStates state = BehaviorStates.FAILURE;
            CandiceAIController agent = rootNode.aiController;
            try
            {
                if (agent.isDead)
                    state = BehaviorStates.SUCCESS;
            }
            catch (Exception e)
            {
                state = BehaviorStates.FAILURE;
                if (CandiceConfig.enableDebug)
                    Debug.LogError("DefaultBehaviors.IsDead: " + e.Message);
            }
            return state;
        }
        public static BehaviorStates SetMoveTarget(BehaviorNode rootNode)
        {
            BehaviorStates state = BehaviorStates.FAILURE;
            CandiceAIController agent = rootNode.aiController;
            try
            {
                if (agent.mainTarget != null)
                {
                    agent.moveTarget = agent.mainTarget;
                    agent.movePoint = agent.moveTarget.transform.position;
                    state = BehaviorStates.SUCCESS;
                }
                else
                {
                    if(CandiceConfig.enableDebug)
                    {
                        Debug.LogError("DefaultBehaviors.SetMoveTarget: Main Target is NULL");
                    }
                }
            }
            catch (Exception e)
            {
                state = BehaviorStates.FAILURE;
                if (CandiceConfig.enableDebug)
                    Debug.LogError("DefaultBehaviors.SetMoveTarget: " + e.Message);
            }
            return state;
        }
        public static BehaviorStates SetAttackTarget(BehaviorNode rootNode)
        {
            BehaviorStates state = BehaviorStates.FAILURE;
            CandiceAIController agent = rootNode.aiController;
            try
            {
                if (agent.mainTarget != null)
                {
                    agent.attackTarget = agent.mainTarget;
                    state = BehaviorStates.SUCCESS;
                }
                else
                {
                    if (CandiceConfig.enableDebug)
                    {
                        Debug.LogError("DefaultBehaviors.SetAttackTarget: Main Target is NULL");
                    }
                }
            }
            catch (Exception e)
            {
                state = BehaviorStates.FAILURE;
                if (CandiceConfig.enableDebug)
                    Debug.LogError("DefaultBehaviors.SetAttackTarget: " + e.Message);
            }
            return state;
        }
        #endregion

        #region ACTION TASKS
        public static BehaviorStates Idle(BehaviorNode rootNode)
        {
            BehaviorStates state = BehaviorStates.SUCCESS;
            if(rootNode.aiController.hasAnimations)
            {
                if (rootNode.aiController.animationType == AnimationType.CodeBased)
                {
                    AnimatorClipInfo[] clipInfo = rootNode.aiController.Animator.GetCurrentAnimatorClipInfo(0);

                    if (!rootNode.aiController.currentAnimation.Equals(clipInfo[0].clip.name))
                    {
                        rootNode.aiController.currentAnimation = (rootNode.aiController.idleAnimationName);
                        rootNode.aiController.Animator.Play(rootNode.aiController.idleAnimationName);
                    }
                }
                else
                {
                    if(!rootNode.aiController.isPlayerControlled)
                        rootNode.aiController.Animator.SetBool(rootNode.aiController.moveTransitionParameter, false);
                }
            }
            return state;
        }
        public static BehaviorStates InitVariables(BehaviorNode rootNode)
        {
            BehaviorStates state = BehaviorStates.SUCCESS;
            rootNode.aiController.isMoving = false;
            return state;
        }
        public static BehaviorStates LookAt(BehaviorNode rootNode)
        {
            BehaviorStates state = BehaviorStates.SUCCESS;
            CandiceAIController agent = rootNode.aiController;
            try
            {
                if (agent.moveType == MovementType.STATIC && agent.is3D && agent.pathfindSource != PathfindSource.Candice && agent.pathfindSource != PathfindSource.UnityNavmesh) 
                {
                    agent.transform.LookAt(agent.mainTarget.transform);
                }
            }
            catch (Exception e)
            {
                if (CandiceConfig.enableDebug)
                    Debug.LogError("DefaultBehaviors.LookAt: " + e.Message);
                state = BehaviorStates.FAILURE;
            }
            return state;
        }
        public static BehaviorStates RotateTo(BehaviorNode rootNode)
        {
            BehaviorStates state = BehaviorStates.SUCCESS;
            try
            {
                float desiredAngle = 180;
                int direction = 1;
                float angle = Vector3.Angle((rootNode.aiController.mainTarget.transform.position - rootNode.aiController.transform.position), rootNode.aiController.transform.right);
                if (angle > 90)
                    angle = 360 - angle;
                if (angle > desiredAngle)
                    direction = -1;

                float rotation = (direction * rootNode.aiController.rotationSpeed) * Time.deltaTime;
                rootNode.aiController.transform.Rotate(0, rotation, 0);
            }
            catch(Exception e)
            {
                state = BehaviorStates.FAILURE;
                if(CandiceConfig.enableDebug)
                    Debug.LogError("DefaultBehaviors.RotateTo: " + e.Message);
            }
            
            return state;
        }
        public static BehaviorStates MoveTo(BehaviorNode rootNode)
        {
            CandiceAIController agent = rootNode.aiController;
            agent.isMoving = true;
            if(rootNode.aiController.hasAnimations)
            {
                if (rootNode.aiController.animationType == AnimationType.CodeBased)
                {
                    if (!agent.currentAnimation.Equals(agent.moveAnimationName))
                    {
                        agent.currentAnimation = agent.moveAnimationName;
                        agent.Animator.Play(agent.moveAnimationName);
                    }
                }
                else
                    agent.Animator.SetBool(agent.moveTransitionParameter, true);
            }
                
            //rootNode.aiController.Animator.SetFloat("characterSpeed", rootNode.aiController.movementSpeed);
            BehaviorStates state = BehaviorStates.SUCCESS;
            try
            {
                if(agent.pathfindSource == PathfindSource.None)
                {
                    agent.MoveTo(agent.moveTarget.transform.position, agent.statMoveSpeed.value, agent.is3D);
                }
                else if(agent.pathfindSource == PathfindSource.Candice)
                {
                    agent.StartFinding();
                }
                else if (agent.pathfindSource == PathfindSource.UnityNavmesh)
                {
                    agent.StartMoveNavMesh(agent.moveTarget);
                }
            }
            catch (Exception e)
            {
                state = BehaviorStates.FAILURE;
                if (CandiceConfig.enableDebug)
                    Debug.LogError("DefaultBehaviors.MoveTo: " + e.Message);
            }

            return state;
        }



        public static BehaviorStates Die(BehaviorNode rootNode)
        {
            BehaviorStates state = BehaviorStates.FAILURE;
            CandiceAIController agent = rootNode.aiController;
            try
            {
                agent._stopBehaviorTree = true;
                if (agent.enableRagdollOnDeath)
                    agent.EnableRagdoll();
                else
                {
                    if (rootNode.aiController.hasAnimations)
                    {
                        if (rootNode.aiController.animationType == AnimationType.CodeBased)
                        {
                            if (!agent.currentAnimation.Equals(agent.deadAnimationName))
                            {
                                agent.currentAnimation = agent.deadAnimationName;
                                agent.Animator.Play(agent.deadAnimationName);
                            }
                        }
                        else
                            agent.Animator.SetBool(agent.deadAnimationName, true);
                    }
                }
                    
                state = BehaviorStates.SUCCESS;
            }
            catch (Exception e)
            {
                state = BehaviorStates.FAILURE;
                if (CandiceConfig.enableDebug)
                    Debug.LogError("DefaultBehaviors.Die: " + e.Message);
            }
            return state;
        }


        public static BehaviorStates Attack(BehaviorNode rootNode)
        {
            BehaviorStates state = BehaviorStates.SUCCESS;
            CandiceAIController agent = rootNode.aiController;
            try
            {
                if (agent.hasAttackAnimation)
                {
                    if (rootNode.aiController.animationType == AnimationType.CodeBased)
                    {
                        if (!agent.currentAnimation.Equals(agent.attackAnimationName))
                        {
                            agent.currentAnimation = agent.attackAnimationName;
                            agent.Animator.Play(agent.attackAnimationName);
                        }
                    }
                    else
                        agent.Animator.SetTrigger(agent.attackAnimationName);
                }
                else
                {
                    if (agent.attackType == AttackType.Melee)
                        agent.Attack();
                    else
                        agent.AttackRange();
                }
            }
            catch (Exception e)
            {
                state = BehaviorStates.FAILURE;
                if (CandiceConfig.enableDebug)
                    Debug.LogError("DefaultBehaviors.Attack: " + e.Message);
            }


            //rootNode.aiController.Animator.SetBool();
            return state;
        }

        public static BehaviorStates Patrol(BehaviorNode rootNode)
        {
            BehaviorStates state = BehaviorStates.SUCCESS;
            try
            {
                rootNode.aiController.Patrol();
            }
            catch (Exception e)
            {
                state = BehaviorStates.FAILURE;
                if (CandiceConfig.enableDebug)
                    Debug.LogError("DefaultBehaviors.Patrol: " + e.Message);
            }
            return state;
            #endregion



        }
    }

}
