/*
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace ViridaxGameStudios.AI
{
    #region ENUMS
    public enum PatrolType
    {
        PatrolPoints,
        Waypoints,
    }
    public enum AttackType
    {
        Melee,
        Range,
    }
    public enum MovementType
    {
        STATIC,
        DYNAMIC,
        TILE_BASED
    }
    public enum PathfindSource
    {
        None,
        Candice,
        UnityNavmesh,
    }
    public enum SensorType
    {
        Sphere,
        Box,
    }
    public enum AnimationType
    {
        TransitionBased,
        CodeBased,
    }
    #endregion

    public class CandiceAIController : Character
    {
        #region Member Variables
        /*
         * General Variables
         */
        public int _agentID;//The ID of the agent. This is assigned by the Candice AI Manager.
        public BehaviorTree _behaviorTree;//The Behavior Tree Object that will control this agent.
        private ObstacleAvoidance _obstacleAvoidance;//Obstacle avoidance module to allow the agent to move and evade obstacles.
        private ObjectScanner _objectScanner;//The Object Scannner module that will handle detection of objects
        public SensorType _sensorType;//This enum determines what kind of sensor the agent will use, 

        public List<GameObject> _enemies;//List containing all detected enemy gameobjects.
        public List<GameObject> _allies;//List containing all detected ally gameobjects.
        public GameObject player;//The detected player gameobject.
        public bool _stopBehaviorTree = false;//boolean condition to stop the behavior tree from executing. Setting this to true will resume the behavior tree.
        
        bool _playerDead = false;

        /*
         * Detection Variables
         */
        public float _detectionRadius = 10f;//How far away around the agent it will be able to detect objects.
        public float _detectionHeight = 2f;//How high up the agent can detect objects.
        public float _lineOfSight = 180.0f;//The angle around the agent that it will be able to detect objects.
        public bool _enemyFound = false;//Bolean to know if the enemy has been found    
        public bool _allyFound = false;//Boolean to know if an ally has been found
        public bool _playerFound = false;//Boolean to know if the player has been found
        public float _obstacleAvoidanceDistance = 3f;//Obstacle Avoidance Distance. How far the agent will detect objects to avoid.
        public float _obstacleAvoidanceAreaOfEffect = 0.5f;//minimum distance from an obstacle the agent will maintain while evading it.

        /*
         * Patrol Variables
         */
        public List<GameObject> _patrolPoints = new List<GameObject>();//List containing all the patrol gameobjects that the agent will use to patrol. Note: this is controlled by the PatrolType variable. the agent can only use one type at a time.
        public int _patrolCount = 0;//The current index in the _patrolPoints List that the agent is moving towards.
        public bool _patrolInOrder = true;//Whether the agent should patrol through the _patrolPoints List in order. If false, the agent will randomly choose a point each time it reaches its current goal.
        public bool _isPatrolling = false;//If set to false, the agent will not patrol. If true, the agent will patrol.
        public bool _pointReached = false;//Boolean to know if the agent reached to targeted patrol point.
        public PatrolType _patrolType;// Enum to determine if the agent will use the PatrolPoints system or the Waypoints system.
        public Waypoint _waypoint;//The first Waypoint that the agent will move to. Note: this is controlled by the PatrolType variable. the agent can only use one type at a time.

        /*
         * Pathfinding Variables
         */
        Path _path;//The path that the Agent will use to follow.
        public float _minPathUpdateTime = .2f;//Minimum time it will take for the agent before attempting to request a new updated path from Candice.
        public float _pathUpdateMoveThreshold = .5f;// Minimum distance the target can move by before requesting a new Updated path from Candice.
        public float _turnSpeed;//The speed the agent will turn between waypoints by when pathfinding.
        public float _turnDist;//The ditance the agent will start to turn while moving to the next node.
        public float _stoppingDist;//How far away from the target the agent will start to slow down and stop.
        Coroutine _updatePathCoroutine;//Coroutine to update the path bsed on the _minPathUpdateTime variable.
        Coroutine _followASTARCoroutine;// Coroutine to begin following the ASTAR path returned by Candice AI Manager.
        bool _followingPath;//Whether or not the agent is following a path.

        /*
         * Tile-Based Movement Variables
         */
        public bool _fallingDown = false;//Whether or not the agent is falling down.
        public bool _jumpingUp = false;//Whether or not the agent is jumping up.
        public bool _movingEdge = false;//Whether or not the agent is moving to the edge of its current tile.
        public Vector3 _jumpTarget;//The target where the agent is going to jump to.
        public List<Tile> _selectableTiles = new List<Tile>();//The tiles that the agent can currently select and potentially move to.
        public Stack<Tile> _tilePath = new Stack<Tile>();//The path of tiles that the agent will follow to reach its target tile.
        public Tile currentTile;//The current tile that the agent is standing on.
        public Tile collidedObject;//The latest tile that the agent collided with. This is usually the current tile, and is mainly used to prevent null exceptions.
        public bool isSelected = false;//Whether or not this agent has been selected by the Player or AI.
        public int movePoints = 5;//How many tiles in any direction the agent can move.
        public float jumpHeight = 2;//Maximum height of an obstacle or tile that the agent can jump over.
        public float jumpVelocity = 4.5f;//How quickly the agent will jump and reach the target. Basically the Jumping speed.
        public bool turn = false;//Whether or not it is this agents turn to perform an action.
        #endregion


        #region Main Methods
        private void Awake()
        {

        }//End Awake()


        public override void Start()
        {
            //Call start from the base class.
            base.Start();
            _obstacleAvoidance = new ObstacleAvoidance();
            _objectScanner = new ObjectScanner(transform, onObjectFound);
            ragdoll = GetComponentsInChildren<Rigidbody>();
            
            //Check if there is a Candice AI Manager Component in the scene.
            CandiceAIManager candice = FindObjectOfType<CandiceAIManager>();
            if (candice == null)
            {
                Debug.LogError(CandiceConfig.CONTROLLER_NAME + ": You need to attach a Candice AI Manager Component to an Empty GameObject.");
            }
            if (is3D)
            {
                if (enableRagdoll)
                {
                    EnableRagdoll();
                }


            }
            if (healthBar != null)
            {
                healthBar.SetAgentName(name);
            }
            if (!isPlayerControlled)
            {
                if (_behaviorTree != null)
                {
                    if (_behaviorTree.behaviorTree != null)
                    {
                        _behaviorTree.CreateBehaviorTree(this);
                    }
                }
            }

            //Subscribe to Events
            CandiceAIManager.getInstance().OnDestinationReached += onDestinationReached;
            CandiceAIManager.getInstance().OnCharacterDead += onCharacterDead;
            CandiceAIManager.getInstance().OnPlayerHealthLow += onPlayerHealthLow;
            CandiceAIManager.getInstance().OnPlayerDetected += onPlayerDetected;
            //Register this agent with Candice
            CandiceAIManager.getInstance().RegisterAgent(gameObject, onRegistrationComplete);

        }//End Start()

        void LateUpdate()
        {
            if (isPlayerControlled)
            {
                if (moveType == MovementType.TILE_BASED)
                {
                    if (isSelected)
                    {
                        currentTile = CandiceAIManager.getInstance().GetCurrentTile(gameObject, collidedObject);
                        CandiceAIManager.getInstance().FindSelectableTiles(currentTile, movePoints, onSelectableTilesFound);
                        CheckMouse();
                        if (_followingPath)
                        {
                            FollowSimplePath();
                        }
                    }
                }
                else
                {
                    ProcessInput();
                }

            }
            else
            {
                DetectionRequest req = new DetectionRequest(_sensorType, enemyTags, allyTags, _detectionRadius, _detectionHeight, _lineOfSight, is3D);
                _objectScanner.ScanForObjects(req);//Scan for objects and ultimately enemies using the _detectionRadius.
                if (_behaviorTree != null)
                {
                    if (_behaviorTree.rootNode != null)
                        _behaviorTree.rootNode.Evaluate();
                }
            }
        }
        private void OnAnimatorIK(int layerIndex)
        {
            if (enableHeadLook && headLookTarget != null)
            {
                Animator.SetLookAtPosition(headLookTarget.transform.position);//Enable the agent to look at the target object
                Animator.SetLookAtWeight(headLookIntensity);//Set the intensity of how much the agent will turn their head to look at the target.
            }
        }
        #endregion


        #region Override Methods
        public override void CharacterDead()
        {
            //Unsubscribe Events
            Debug.Log("Character Dead");
            CandiceAIManager.getInstance().OnDestinationReached -= onDestinationReached;
            CandiceAIManager.getInstance().OnCharacterDead -= onCharacterDead;
            CandiceAIManager.getInstance().OnPlayerHealthLow -= onPlayerHealthLow;
            CandiceAIManager.getInstance().OnPlayerDetected -= onPlayerDetected;

            CandiceAIManager.getInstance().CharacterDead(gameObject);
            base.CharacterDead();
        }

        #endregion

        #region Helper Methods 
        private void onRegistrationComplete(bool isRegistered, int id)
        {
            if (isRegistered)
            {
                _agentID = id;
                /*
                if (!isPlayerControlled)
                {
                    if (healthBar != null)
                        healthBar.SetAgentName("AI Agent " + id);
                }
                */
                if (CandiceConfig.enableDebug)
                    Debug.Log("Agent " + _agentID + " successfully registered with Candice.");
            }
            //Debug.Log("Agent " + _agentID + " successfully registered with Candice.");

        }

        public void onPlayerHealthLow(GameObject player)
        {
            if (CandiceConfig.enableDebug)
                Debug.Log("The players health is low");
        }
        void onCharacterDead(GameObject go)
        {
            if (gameObject.tag.Equals("Player"))
            {
                _playerDead = true;
            }
        }
        public void EnableRagdoll()
        {
            if (ragdoll != null && ragdoll.Length > 0)
            {
                foreach (Rigidbody rb in ragdoll)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    //rb.WakeUp();
                    //rb.gameObject.GetComponent<Collider>().isTrigger = false;
                    rb.gameObject.GetComponent<Collider>().enabled = true;
                }
            }
            if(hasAnimations)
            {
                Animator.enabled = false;
            }
            
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            //gameObject.GetComponent<Rigidbody>().isKinematic = true;

        }
        void DisableRagdoll()
        {
            if (hasAnimations)
            {
                Animator.enabled = true;
            }
            if (ragdoll != null && ragdoll.Length > 0)
            {
                foreach (Rigidbody rb in ragdoll)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                    //rb.Sleep();
                    //rb.gameObject.GetComponent<Collider>().isTrigger = true;
                    rb.gameObject.GetComponent<Collider>().enabled = false;
                }
            }
            if (hasAnimations)
            {
                Animator.enabled = true;
            }
            gameObject.GetComponent<Collider>().enabled = true;
            //gameObject.GetComponent<Rigidbody>().useGravity = true;
            //gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        public void StartFinding()
        {
            if (_updatePathCoroutine == null)
            {
                //Start moving the agent using Candice's pathfinding module.
                _updatePathCoroutine = StartCoroutine(UpdatePath());
            }
        }
        public void StopFinding()
        {
            //Stop moving the agent using Candice's pathfinding module.
            if(_updatePathCoroutine != null)
                StopCoroutine(_updatePathCoroutine);
            if(_followASTARCoroutine != null)
                StopCoroutine(_followASTARCoroutine);

            _followASTARCoroutine = null;
            _updatePathCoroutine = null;
            _path = null;
        }

        void onDestinationReached(CandiceAIController agent)
        {
            if (agent._agentID == _agentID)
            {
                if(isPlayerControlled)
                    fsm.ChangeToState(idleState);
                else if (hasAnimations)
                {
                    if (animationType == AnimationType.CodeBased)
                    {
                        AnimatorClipInfo[] clipInfo = Animator.GetCurrentAnimatorClipInfo(0);

                        if (!idleAnimationName.Equals(clipInfo[0].clip.name))
                        {
                            Animator.Play(idleAnimationName);
                        }
                    }
                    else
                        Animator.SetBool(moveTransitionParameter, false);

                }
                if(moveType == MovementType.TILE_BASED)
                    RemoveSelectableTiles();
                //isMoving = false;
                //moveToTile = false;



                if (CandiceConfig.enableDebug)
                    Debug.Log(_agentID + ": Destination reached");
            }

        }
        void onObjectFound(DetectionResults results)
        {
            if (results.enemies != null && results.enemies.Count > 0)
            {
                _enemyFound = true;
                _enemies = results.enemies;
            }
            else
            {
                _enemyFound = false;
            }

            if (results.allies != null && results.allies.Count > 0)
            {
                _allyFound = true;
                _allies = results.allies;
            }
            else
            {
                _allyFound = false;
            }

            if (results.player != null && results.player.Count > 0)
            {
                _playerFound = true;
                player = results.player[0];
                CandiceAIManager.getInstance().PlayerDetected(gameObject, player);
            }
            else
            {
                _playerFound = false;
            }



            if (!_isPatrolling && !_enemyFound)
            {
                attackTarget = null;
                /*
                if (!isMoving)
                {
                    moveTarget = null;
                    movePoint = new Vector3();
                }
                */
            }
        }
        void TargetLost()
        {

        }
        void onPlayerDetected(GameObject source, GameObject player)
        {
            //Implement logic here for when the player is detected
            //Debug.Log("Player Detected");
        }
        public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                _path = new Path(waypoints, transform.position, _turnDist, _stoppingDist);
                if(_followASTARCoroutine == null)
                {
                    _followASTARCoroutine = StartCoroutine(FollowAStarPath());
                }
                else
                {
                    StopCoroutine(_followASTARCoroutine);
                    _followASTARCoroutine = StartCoroutine(FollowAStarPath());
                }
                
            }
        }


        IEnumerator UpdatePath()
        {
            if (Time.timeSinceLevelLoad < .3f)
            {
                yield return new WaitForSeconds(.3f);
            }
            CandiceAIManager.getInstance().RequestASTARPath(new PathRequest(transform.position, moveTarget.transform.position, OnPathFound));
            float sqrMoveThreshold = _pathUpdateMoveThreshold * _pathUpdateMoveThreshold;
            Vector3 targetPosOld = moveTarget.transform.position;

            while (true)
            {
                yield return new WaitForSeconds(_minPathUpdateTime);
                if (moveTarget != null)
                {
                    if ((moveTarget.transform.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
                    {
                        CandiceAIManager.getInstance().RequestASTARPath(new PathRequest(transform.position, moveTarget.transform.position, OnPathFound));
                        targetPosOld = moveTarget.transform.position;
                    }
                }
                if (!isMoving)
                {
                    StopCoroutine(_updatePathCoroutine);
                    StopCoroutine(_followASTARCoroutine);
                    _updatePathCoroutine = null;
                }
            }

        }

        IEnumerator FollowAStarPath()
        {
            bool followingPath = true;
            int pathIndex = 0;
            if (is3D)
                transform.LookAt(_path.lookPoints[0]);
            float speedPercent = 1;
            while (followingPath)
            {
                float distance = Vector3.Distance(transform.position, moveTarget.transform.position);
                if(distance <= _detectionRadius)
                {
                    Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
                    while (_path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
                    {
                        if (pathIndex == _path.finishLineIndex)
                        {
                            followingPath = false;
                            onDestinationReached(this);
                            break;
                        }
                        else
                        {
                            pathIndex++;
                            if (is3D)
                                transform.LookAt(_path.lookPoints[pathIndex]);
                        }
                    }
                    if (followingPath)
                    {
                        if (pathIndex >= _path.slowDownIndex && _stoppingDist > 0)
                        {
                            speedPercent = Mathf.Clamp01(_path.turnBoundaries[_path.finishLineIndex].DistanceFromPoint(pos2D) / _stoppingDist);
                            if (speedPercent < 0.01f)
                            {
                                followingPath = false;
                            }
                        }

                        if (is3D)
                        {
                            Quaternion targetRotation = Quaternion.LookRotation(_path.lookPoints[pathIndex] - transform.position);
                            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);
                            transform.Translate(Vector3.forward * Time.deltaTime * statMoveSpeed.value * speedPercent, Space.Self);
                        }
                        else
                        {
                            //Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                            //Debug.Log(path.lookPoints[pathIndex]);
                            //Vector3 lookPoint = path.lookPoints[pathIndex];
                            //transform.position += path.lookPoints[pathIndex];
                            MoveTo(_path.lookPoints[pathIndex], statMoveSpeed.value, is3D);
                            //transform.Translate( Vector3.forward * Time.deltaTime * movementSpeed * speedPercent, Space.Self);
                        }
                    }
                }
                else
                {
                    StopFinding();
                }

                
                yield return null;
            }
        }

        public void RemoveSelectableTiles()
        {
            //Reset the selctable tiles list

            if (currentTile != null)
            {
                currentTile.current = false;
                currentTile = null;
            }

            foreach (Tile tile in _selectableTiles)
            {
                tile.Reset();
            }
            _selectableTiles.Clear();
            CandiceAIManager.getInstance().ComputeAdjacencyList(jumpHeight);
        }

        void CheckMouse()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Tile")
                    {
                        Tile t = hit.collider.GetComponent<Tile>();
                        if (t.selectable)
                        {
                            //MoveToTile(t);
                            //movePoint.GetComponent<Collider>().enabled = true;
                            //movePoint.transform.position = hit.point;
                            moveTarget = hit.collider.gameObject;
                            movePoint = hit.point;
                            //moveToTile = true;
                            MoveToTile(t);
                            //CandiceAIManager.SimpleMove(transform, target.transform, movementSpeed, true);
                        }
                    }
                    else
                    {
                        CandiceAIController agent = hit.collider.gameObject.GetComponent<CandiceAIController>();
                        if (agent != null)
                        {
                            if (!_tacticsPlayer.IsAlly(agent))
                            {
                                transform.LookAt(agent.transform);
                                if (hasAnimations)
                                {
                                    if (animationType == AnimationType.CodeBased)
                                    {
                                        AnimatorClipInfo[] clipInfo = Animator.GetCurrentAnimatorClipInfo(0);

                                        if (!attackAnimationName.Equals(clipInfo[0].clip.name))
                                        {
                                            Animator.Play(attackAnimationName);
                                        }
                                    }
                                    else
                                        agent.Animator.SetTrigger(attackTransitionParameter);

                                }
                                
                            }
                        }
                    }
                }
            }
        }
        void onSelectableTilesFound(List<Tile> selectableTiles)
        {
            this._selectableTiles = selectableTiles;
        }
        public void onBFSPathFound(Stack<Tile> bfsPath)
        {
            if (bfsPath != null)
            {
                _tilePath = bfsPath;
                //Put code to follow the path
                _followingPath = true;



            }
        }
        public static void SimpleMove(Transform transform, Vector3 target, float movementSpeed, bool is3D)
        {
            if (is3D)
            {
                transform.position += transform.forward * movementSpeed * Time.deltaTime;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);
            }
        }
        public static void SimpleMove(Transform transform, Transform target, float movementSpeed, bool is3D)
        {
            SimpleMove(transform, target.position, movementSpeed, is3D);

        }
        public void MoveToTile(Tile tile)
        {
            //CandiceAIManager.RequestBFSPath(tile, onBFSPathFound);
            _tilePath.Clear();
            tile.target = true;
            //isMoving = true;

            Tile next = tile;
            while (next != null)
            {
                _tilePath.Push(next);
                next = next.parent;
            }
            _tilePath.Pop();
            _followingPath = true;

            //Debug.Log("Tiles: " + tilePath.Count);
        }
        void FollowSimplePath()
        {
            _followingPath = true;
            if (_tilePath.Count > 0)
            {
                Tile t = _tilePath.Peek();
                Vector3 target = t.transform.position;
                //Calculate the units position on top of the target tile
                target.y = transform.position.y;

                if (Vector3.Distance(transform.position, target) >= 0.2f)
                {
                    if (hasAnimations)
                    {
                        if (animationType == AnimationType.CodeBased)
                        {
                            AnimatorClipInfo[] clipInfo = Animator.GetCurrentAnimatorClipInfo(0);

                            if (!moveAnimationName.Equals(clipInfo[0].clip.name))
                            {
                                Animator.Play(moveAnimationName);
                            }
                        }
                        else
                            Animator.SetBool(moveTransitionParameter, true);


                    }
                    //Locomotion
                    transform.LookAt(target, Vector3.up);
                    transform.position += transform.forward * statMoveSpeed.value * Time.deltaTime;
                }
                else
                {
                    if (_tilePath.Count == 1)
                    {
                        transform.position = target;
                    }
                    _tilePath.Pop();

                }
            }
            else
            {
                _followingPath = false;
                CandiceAIManager.getInstance().DestinationReached(this);
                //RemoveSelectableTiles();
                onDestinationReached(this);
                
            }
            //Debug.Log("Following: " )
            //followPathCoroutine = null;
        }
        public void MoveForward()
        {
            if (is3D)
            {
                if(moveType == MovementType.STATIC)
                {
                    transform.position += transform.forward * statMoveSpeed.value * Time.deltaTime;
                }
                else if (moveType == MovementType.DYNAMIC)
                {
                    _obstacleAvoidance.Move(moveTarget.transform, transform, halfHeight + _obstacleAvoidanceAreaOfEffect, statMoveSpeed.value, is3D, _obstacleAvoidanceDistance);
                }
                
                //rb.velocity += direction *( movementSpeed * Time.deltaTime);
            }
            else
            {
                if (CandiceConfig.enableDebug)
                    Debug.LogError("cannot call MoveForward if is3D is false");
            }

        }

        public void MoveTo(Vector3 target, float movementSpeed, bool is3D)
        {
            if (is3D)
            {
                //transform.position += Vector3.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);
                MoveForward();
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);
            }
        }
        public void Patrol()
        {
            if(_patrolType == PatrolType.PatrolPoints)
            {
                NormalPatrol();
            }
            else if( _patrolType == PatrolType.Waypoints)
            {
                WaypointPatrol();
            }
            
        }
        void NormalPatrol()
        {
            if (_pointReached)
            {
                if (_patrolInOrder)
                {
                    if (_patrolCount < _patrolPoints.Count - 1)
                    {
                        _patrolCount++;
                    }
                    else
                    {
                        _patrolCount = 0;
                    }
                }
                else
                {
                    _patrolCount = UnityEngine.Random.Range(0, _patrolPoints.Count);
                }
                mainTarget = _patrolPoints[_patrolCount];
                moveTarget = mainTarget;
                movePoint = moveTarget.transform.position;
                _pointReached = false;
            }
            else
            {
                mainTarget = _patrolPoints[_patrolCount];
                moveTarget = mainTarget;
            }
        }
        void WaypointPatrol()
        {
            if(Vector3.Distance(transform.position,movePoint) < .5f)
            {
                _pointReached = true;
            }
            if(_pointReached)
            {
                _waypoint = _waypoint.nextWaypoint;
                if(_waypoint != null)
                {
                    mainTarget = _waypoint.gameObject;
                    moveTarget = _waypoint.gameObject;
                    movePoint = _waypoint.GetPosition();
                }
                _pointReached = false;
                
            }
            else
            {
                if(_waypoint == null)
                {
                    Debug.LogError("No waypoint assigned.");
                    return;
                }
                mainTarget = _waypoint.gameObject;
                moveTarget = _waypoint.gameObject;
                movePoint = _waypoint.GetPosition();
            }
        }
        #endregion
        #region Override Methods
        public void OnDrawGizmos()
        {
            if (_path != null)
            {
                _path.DrawWithGizmos();
            }

            if (_path != null)
            {
                for (int i = 0; i < _path.lookPoints.Length; i++)
                {
                    Gizmos.color = Color.white;
                    if (i != 0)
                    {
                        Gizmos.DrawLine(_path.lookPoints[i - 1], _path.lookPoints[i]);
                    }

                }
            }
        }
        void OnCollisionEnter(Collision col)
        {
            GameObject go = col.gameObject;
            if (go.tag == "Tile")
            {
                Tile t = go.GetComponent<Tile>();
                if (t != null && collidedObject != t)
                {
                    collidedObject = t;
                    collidedObject.selectable = false;
                }

                //currentTile = collidedObject;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (_isPatrolling && moveTarget != null)
            {
                if (other.gameObject.tag.Equals("Patrol Point") && other.gameObject.name.Equals(moveTarget.gameObject.name))
                {
                    _pointReached = true;
                    if (CandiceConfig.enableDebug)
                        Debug.Log("Patrol Point reached");
                }
            }

            if (other.gameObject.name == "movePoint" && isPlayerControlled)
            {
                CandiceAIManager.getInstance().DestinationReached(this);
            }

        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isPatrolling)
            {
                if (other.gameObject.tag.Equals("PatrolPoint") && other.gameObject.name.Equals(moveTarget.gameObject.name))
                {
                    _pointReached = true;
                    if (CandiceConfig.enableDebug)
                        Debug.Log("Patrol Point reached");
                }
            }
            if (other.gameObject.name == "movePoint" && isPlayerControlled)
            {
                CandiceAIManager.getInstance().DestinationReached(this);
            }


        }
        #endregion
    }
    [Serializable]
    public class SpecialAbility
    {
        public string abilityName = "<<Not Set>>";
        public KeyCode abilityKey;
    }


}