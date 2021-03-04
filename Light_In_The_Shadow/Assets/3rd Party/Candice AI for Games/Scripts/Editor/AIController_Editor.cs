using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System;

namespace ViridaxGameStudios.AI
{
    [CustomEditor(typeof(CandiceAIController))]
    public class AIController_Editor : Editor
    {
        #region variables
        private CandiceAIController character;
        private SerializedObject soTarget;

        string[] arrAttackTypes = {"Melee", "Range"};
        string[] arrSettingsTabs = { "General", "Key Mapping", "Relationships" };
        string[] arrAnimationTabs = { "Parameters", "Default", "Movement", "Combat" };
        //string[] arrTabs = { "AI Settings", "Stats", "Detection", "Movement", "Combat", "Animation" };
        GUIContent[] arrTabs = new GUIContent[6];

        private int tabIndex;
        private int settingTabIndex;
        private int enemyTagCount;
        private int patrolPointCount;
        private int allyTagCount;
        GUIStyle guiStyle = new GUIStyle();
        bool showPatrolPoints = false;
        bool showAllyTags = false;
        bool showEnemyTags = false;

        #endregion

        #region Main Methods
        void OnEnable()
        {
            //Store a reference to the AI Controller script
            character = (CandiceAIController)target;
            soTarget = new SerializedObject(character);
            guiStyle.fontSize = 14;
            guiStyle.fontStyle = FontStyle.Bold;

            arrTabs[0] = new GUIContent("Settings",(Texture2D)Resources.Load("Settings"));
            arrTabs[1] = new GUIContent("  Stats",(Texture2D)Resources.Load("Stats"));
            arrTabs[2] = new GUIContent("Detection", (Texture2D)Resources.Load("Detection"));
            arrTabs[3] = new GUIContent("Movement", (Texture2D)Resources.Load("Movement"));
            arrTabs[4] = new GUIContent("Combat", (Texture2D)Resources.Load("Combat"));
            arrTabs[5] = new GUIContent("Animation", (Texture2D)Resources.Load("Animation"));

        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            GUIStyle style = new GUIStyle();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            style.normal.textColor = Color.red;
            Texture2D image = (Texture2D)Resources.Load("CandiceLogo");
            GUIContent label = new GUIContent(image);
            GUILayout.Label(label);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            label = new GUIContent("Candice AI Controller");
            guiStyle.normal.textColor = EditorStyles.label.normal.textColor;
            GUILayout.Label(label, guiStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(6);
            EditorGUI.BeginChangeCheck();

            //tabIndex = GUILayout.Toolbar(tabIndex, arrTabs);
            
            tabIndex = GUILayout.SelectionGrid(tabIndex, arrTabs, 3);
            
            if (EditorGUI.EndChangeCheck())
            {
                GUI.FocusControl(null);
            }
            GUILayout.Space(8);
            GUILayout.BeginVertical("box");
            EditorGUI.BeginChangeCheck();
            switch (tabIndex)
            {
                case 0:
                    DrawAISettingGUI();
                    break;
                case 1:
                    DrawStatsGUI();

                    break;
                case 2:
                    DrawDetectionGUI();
                    break;
                case 3:
                    DrawMovementGUI();
                    break;
                case 4:
                    DrawCombatGUI();
                    break;
                case 5:
                    DrawAnimationGUI();
                    break;
            }
            if (EditorGUI.EndChangeCheck())
            {

            }
            GUILayout.EndVertical();

        }

        

        #region DRAW TAB REGION
        void DrawAISettingGUI()
        {
            
            EditorGUI.BeginChangeCheck();

            //tabIndex = GUILayout.Toolbar(tabIndex, arrTabs);
            settingTabIndex = GUILayout.SelectionGrid(settingTabIndex, arrSettingsTabs, 3);

            if (EditorGUI.EndChangeCheck())
            {
                GUI.FocusControl(null);
            }
            GUILayout.Space(8);
            GUILayout.BeginVertical("box");
            EditorGUI.BeginChangeCheck();
            switch (settingTabIndex)
            {
                case 0:
                    DrawGeneralGUI();
                    break;
                case 1:
                    DrawKeyMapGUI();

                    break;
                case 2:
                    DrawRelationshipGUI();
                    break;
            }
            GUILayout.EndVertical();
            if (EditorGUI.EndChangeCheck())
            {
            }
            
            



        }

        private void DrawGeneralGUI()
        {
            GUIContent label;
            label = new GUIContent("Agent ID", "The unique ID of the agent. Automatically generated at runtime.");
            EditorGUILayout.TextField(label, character._agentID.ToString());
            label = new GUIContent("Is 3D", "Uncheck if this character is in 2D space.");
            character.is3D = EditorGUILayout.Toggle(label, character.is3D);
            label = new GUIContent("Health Bar", "");
            character.healthBar = (HealthBarScript)EditorGUILayout.ObjectField(label, character.healthBar, typeof(HealthBarScript), true);
            label = new GUIContent("Behavior Tree", "");
            character._behaviorTree = (BehaviorTree)EditorGUILayout.ObjectField(label, character._behaviorTree, typeof(BehaviorTree), true);
            label = new GUIContent("Stop Behavior Tree", "Stops the current active behavior tree from executing.");
            character._stopBehaviorTree = EditorGUILayout.Toggle(label, character._stopBehaviorTree);
            label = new GUIContent("Is Player Controlled", "Whether or not this AI is controlled by the player.");
            character.isPlayerControlled = EditorGUILayout.Toggle(label, character.isPlayerControlled);
            label = new GUIContent("Camera", "The Main Camera GameObject.");
            character.cam = (Camera)EditorGUILayout.ObjectField(label, character.cam, typeof(Camera), true);
            label = new GUIContent("Rig", "The rig that contains all the bones of the character. This is a prerequisite for enabling ragdoll.");
            character.rig = (GameObject)EditorGUILayout.ObjectField(label, character.rig, typeof(GameObject), true);
            label = new GUIContent("Enable ragdoll", "Enable ragdoll from the start.");
            character.enableRagdoll = EditorGUILayout.Toggle(label, character.enableRagdoll);
            label = new GUIContent("Enable Ragdoll on Death", "Enable ragdoll when the character dies.");
            character.enableRagdollOnDeath = EditorGUILayout.Toggle(label, character.enableRagdollOnDeath);
        }
        void DrawRelationshipGUI()
        {
            GUIContent label = new GUIContent("Faction", "The faction that this character belongs to.");
            //character.faction = (Faction)EditorGUILayout.ObjectField(label, character.faction, typeof(Faction), true);
            label = new GUIContent("Allies:", "All the Tags that the character will consider as an ally. NOTE: The default reaction is to follow.");
            EditorGUILayout.LabelField(label, guiStyle);
            showAllyTags = EditorGUILayout.Foldout(showAllyTags, label);
            if (showAllyTags)
            {
                allyTagCount = character.allyTags.Count;
                allyTagCount = EditorGUILayout.IntField("Size: ", allyTagCount);

                if (allyTagCount != character.allyTags.Count)
                {
                    int i = 0;
                    while (allyTagCount > character.allyTags.Count)
                    {
                        character.allyTags.Add("Ally" + i);
                        i++;
                    }
                    while (allyTagCount < character.allyTags.Count)
                    {
                        character.allyTags.RemoveAt(character.allyTags.Count - 1);
                    }
                }

                for (int i = 0; i < character.allyTags.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Element " + i);
                    string tag = "";
                    tag = EditorGUILayout.TagField(character.allyTags[i]);

                    if (character.enemyTags.Contains(tag))
                    {
                        EditorUtility.DisplayDialog("AI Controller", "Tag '" + tag + "' already added to enemy tags", "OK");
                    }
                    else
                    {
                        character.allyTags[i] = tag;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }
            //Enemy Relationships
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            label = new GUIContent("Enemies", "All the Tags that the character will consider as an enemy. NOTE: The default reaction is to attack.");
            EditorGUILayout.LabelField(label, guiStyle);
            showEnemyTags = EditorGUILayout.Foldout(showEnemyTags, label);
            if (showEnemyTags)
            {
                enemyTagCount = character.enemyTags.Count;
                enemyTagCount = EditorGUILayout.IntField("Size", enemyTagCount);

                if (enemyTagCount != character.enemyTags.Count)
                {
                    int i = 0;
                    while (enemyTagCount > character.enemyTags.Count)
                    {
                        character.enemyTags.Add("Enemy" + i);
                        i++;
                    }
                    while (enemyTagCount < character.enemyTags.Count)
                    {
                        character.enemyTags.RemoveAt(character.enemyTags.Count - 1);
                    }
                }

                for (int i = 0; i < character.enemyTags.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Element " + i);
                    string tag = "";
                    tag = EditorGUILayout.TagField(character.enemyTags[i]);

                    if (character.allyTags.Contains(tag))
                    {
                        EditorUtility.DisplayDialog("AI Controller", "Tag '" + tag + "' already added to ally tags", "OK");
                    }
                    else
                    {
                        character.enemyTags[i] = tag;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }
        }
        void DrawStatsGUI()
        {
            GUIContent label = new GUIContent("Overview Stats", "");
            GUIContent label2 = new GUIContent("", "");
            GUILayout.Label(label, guiStyle);

            label = new GUIContent("Base Hit Points", "The base hit points of the character after all modifiers are added.");
            character.statHitPoints.baseValue = EditorGUILayout.FloatField(label, character.statHitPoints.baseValue);

            label = new GUIContent("Max Hit Points", "The maximum hit points of the character after all modifiers are added.");
            label2 = new GUIContent(character.statHitPoints.value.ToString());
            EditorGUILayout.LabelField(label, label2);
            label = new GUIContent("Current Hit Points", "The current hit points of the character.");
            label2 = new GUIContent(character.currentHP.ToString());

            EditorGUILayout.LabelField(label, label2);
            EditorGUILayout.Space();
            label = new GUIContent("Base Attack Damage", "");
            character.statAttackDamage.baseValue = EditorGUILayout.FloatField(label, character.statAttackDamage.baseValue);

            label = new GUIContent("Base Movement Speed", "");
            character.statMoveSpeed.baseValue = EditorGUILayout.FloatField(label, character.statMoveSpeed.baseValue);
            EditorGUILayout.Space();

            //label = new GUIContent("Stat Multiplier", "This is used to calculate the attack damage based on your Strength, Intelligence and Faith. This variable can be changed during runtime.");
            //character.m_StatMultiplier = EditorGUILayout.Slider(label, character.m_StatMultiplier, 1f, 10);
            GUILayout.Label("Attributes", guiStyle);
            label = new GUIContent(character.statStrength.name);
            AddModifierWindow window = CreateInstance<AddModifierWindow>();
            window.stat = character.statStrength;
            character.statStrength.Draw(window);
            window = CreateInstance<AddModifierWindow>();
            window.stat = character.statIntelligence;
            character.statIntelligence.Draw(window);
            window = CreateInstance<AddModifierWindow>();
            window.stat = character.statFaith;
            character.statFaith.Draw(window);
            window = CreateInstance<AddModifierWindow>();
            window.stat = character.statHitPoints;
            character.statHitPoints.Draw(window);
            window = CreateInstance<AddModifierWindow>();
            window.stat = character.statAttackDamage;
            character.statAttackDamage.Draw(window);
            window = CreateInstance<AddModifierWindow>();
            window.stat = character.statAttackRange;
            character.statAttackRange.Draw(window);
            window = CreateInstance<AddModifierWindow>();
            window.stat = character.statMoveSpeed;
            character.statMoveSpeed.Draw(window);

        }

        
        #endregion

        void DrawMovementGUI()
        {
            //Detection and Head Look Settings
            GUILayout.Label("General Movement Settings", guiStyle);
            GUIContent label;
            EditorGUILayout.ObjectField("Move Target", character.moveTarget, typeof(GameObject), true);
            EditorGUILayout.ObjectField("Main Target", character.mainTarget, typeof(GameObject), true);
            label = new GUIContent("Base Movement Speed", "The base speed at which the agent will move at.");
            character.statMoveSpeed.baseValue = EditorGUILayout.FloatField(label, character.statMoveSpeed.baseValue);
            label = new GUIContent("Base Rotation Speed", "The base speed at which the agent rotate to face its target.");
            character.rotationSpeed = EditorGUILayout.FloatField(label, character.rotationSpeed);
            GUILayout.Space(16);
            label = new GUIContent("Movement Type", "Choose the movement type that this AI agent will use.");
            character.moveType = (MovementType)EditorGUILayout.EnumPopup(label, character.moveType);
            label = new GUIContent("Pathfind Source", "Choose the pathfind source that this AI agent will use.");
            character.pathfindSource = (PathfindSource)EditorGUILayout.EnumPopup(label, character.pathfindSource);
            if (character.pathfindSource == PathfindSource.Candice)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Candice Pathfind Settings", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                label = new GUIContent("Turn Speed", "The speed the agent will turn between waypoints by when pathfinding.");
                character._turnSpeed = EditorGUILayout.FloatField(label, character._turnSpeed);
                label = new GUIContent("Turn Distance", "The ditance the agent will start to turn while moving to the next node.");
                character._turnDist = EditorGUILayout.FloatField(label, character._turnDist);
                label = new GUIContent("Stopping Distance", "How far away the agent will start to come to a halt.");
                character._stoppingDist = EditorGUILayout.FloatField(label, character._stoppingDist);
                label = new GUIContent("Minimum Path Update Time", "Minimum time it will take for the agent before attempting to request a new updated path from Candice.");
                character._minPathUpdateTime = EditorGUILayout.FloatField(label, character._minPathUpdateTime);
                label = new GUIContent("Path Update Move Threshold", "Minimum distance the target can move by before requesting a new Updated path from Candice.");
                character._pathUpdateMoveThreshold = EditorGUILayout.FloatField(label, character._pathUpdateMoveThreshold);
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(16);
            GUILayout.Label("Head Look Settings", guiStyle);
            label = new GUIContent("Enable Head Look:", "Allow the agent to dynamically look at objects.");
            character.enableHeadLook = EditorGUILayout.Toggle(label, character.enableHeadLook);
            label = new GUIContent("Head Look target: ");
            character.headLookTarget = (GameObject)EditorGUILayout.ObjectField(label, character.headLookTarget, typeof(GameObject), true);
            label = new GUIContent("Head Look Intensity:", "How quickly the agent will turn their head to look at objects.");
            character.headLookIntensity = EditorGUILayout.Slider(label, character.headLookIntensity, 0f, 1f);

            //Patrol Settings
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            //GUIContent label = new GUIContent("Patrol Settings");

            //EditorGUILayout.LabelField();
            EditorGUILayout.LabelField("Patrol/Waypoint Settings", guiStyle);
            label = new GUIContent("Patrol Type", "");
            character._patrolType = (PatrolType)EditorGUILayout.EnumPopup(label, character._patrolType);
            character._isPatrolling = EditorGUILayout.Toggle("Is Patrolling", character._isPatrolling);
            label = new GUIContent("Patrol In Order:", "Whether or not the character should patrol each point in order of the list. False will allow the character to patrol randomly.");
            if(character._patrolType == PatrolType.PatrolPoints)
            {
                character._patrolInOrder = EditorGUILayout.Toggle(label, character._patrolInOrder);
                label = new GUIContent("Patrol Points:", "The points in the gameworld where you want the character to patrol. They can be anything, even empty gameObjects. Note: Ensure each patrol point is tagged as 'PatrolPoint'");

                patrolPointCount = character._patrolPoints.Count;
                showPatrolPoints = EditorGUILayout.Foldout(showPatrolPoints, label);
                if (showPatrolPoints)
                {
                    label = new GUIContent("Size:");
                    patrolPointCount = EditorGUILayout.IntField(label, patrolPointCount);

                    if (patrolPointCount != character._patrolPoints.Count)
                    {
                        while (patrolPointCount > character._patrolPoints.Count)
                        {
                            character._patrolPoints.Add(null);
                        }
                        while (patrolPointCount < character._patrolPoints.Count)
                        {
                            character._patrolPoints.RemoveAt(character._patrolPoints.Count - 1);
                        }
                    }
                    //EditorGUILayout.Space();
                    for (int i = 0; i < character._patrolPoints.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Element " + i);
                        character._patrolPoints[i] = (GameObject)EditorGUILayout.ObjectField(character._patrolPoints[i], typeof(GameObject), true);
                        EditorGUILayout.EndHorizontal();
                        //EditorGUILayout.Space();
                    }
                }
            }
            else if(character._patrolType == PatrolType.Waypoints)
            {
                label = new GUIContent("Waypoint:", "The first Waypoint that the agent will follow.");

                character._waypoint = (Waypoint)EditorGUILayout.ObjectField(label, character._waypoint, typeof(Waypoint), true);
            }
            
            /*
            */
        }
        void DrawDetectionGUI()
        {
            GUIContent label;
            GUILayout.Label("Detection Settings", guiStyle);

            label = new GUIContent("Sensor Type", "The sensor type this agent will use. Note: Sphere sensor also works with 2D.");
            character._sensorType = (SensorType) EditorGUILayout.EnumPopup(label, character._sensorType);

            label = new GUIContent("Perception Mask", "Layers that the agent must ignore");
            LayerMask tempMask = EditorGUILayout.MaskField(label, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(character.perceptionMask), InternalEditorUtility.layers);
            character.perceptionMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
            label = new GUIContent("Detection Radius", "The radius which the character can detect other objects.");
            character._detectionRadius = EditorGUILayout.FloatField(label, character._detectionRadius);
            label = new GUIContent("Detection Height", "The height at which the agent can detect objects.");
            character._detectionHeight = EditorGUILayout.FloatField(label, character._detectionHeight);
            label = new GUIContent("Line of Sight", "The area where the agent will be able to see objects.");
            character._lineOfSight = EditorGUILayout.FloatField(label, character._lineOfSight);

            GUILayout.Label("Obstacle Avoidance Settings", guiStyle);
            label = new GUIContent("OA Distance", "The maximum distance that the agent will start to avoid detected objects.");
            character._obstacleAvoidanceDistance = EditorGUILayout.FloatField(label, character._obstacleAvoidanceDistance);
            label = new GUIContent("OA AOE", "the Area Of Effect around the agent that must not touch obstacles");
            character._obstacleAvoidanceAreaOfEffect = EditorGUILayout.FloatField(label, character._obstacleAvoidanceAreaOfEffect);
        }
        void DrawKeyMapGUI()
        {
            GUIContent label = new GUIContent("Click to Move", "If selected, click the area to move the agent. If disabled, normal keys will be used (e.g WASD and arrow keys)");
            character.clickToMove = EditorGUILayout.Toggle(label, character.clickToMove);      
            character.keyAttack = (KeyCode)EditorGUILayout.EnumPopup("Attack 1", character.keyAttack);
            character.keyAttack2 = (KeyCode)EditorGUILayout.EnumPopup("Attack 2", character.keyAttack2);
            character.special1 = (KeyCode) EditorGUILayout.EnumPopup("Special 1", character.special1);
            character.special2 = (KeyCode) EditorGUILayout.EnumPopup("Special 2", character.special2);
            character.special3 = (KeyCode) EditorGUILayout.EnumPopup("Special 3", character.special3);
        }
        
        
        
        void DrawCombatGUI()
        {
            GUILayout.Label("Attack Settings", guiStyle);
            GUIContent label = new GUIContent("Attack Type", "");
            character.attackType = (AttackType) EditorGUILayout.EnumPopup(label, character.attackType);
            label = new GUIContent("Attack Range", "The range that the AI will start attacking enemies.");
            character.statAttackRange.baseValue = EditorGUILayout.FloatField(label, character.statAttackRange.baseValue);
            label = new GUIContent("Attack Projectile", "The projectile that the agent will fire.");
            character.attackProjectile = (GameObject)EditorGUILayout.ObjectField(label, character.attackProjectile, typeof(GameObject), true);

            label = new GUIContent("Projectile Spawn Position", "The point where the projectile will spawn. e.g the hand for a spell, or the bow for an arrow.");
            character.spawnPosition = (Transform)EditorGUILayout.ObjectField(label, character.spawnPosition, typeof(Transform), true);

            //character.m_DamageAngle = EditorGUILayout.Slider("Damage Angle:", character.m_DamageAngle, 0, 360f);
            label = new GUIContent("Has Attack Animation", "Whether or not this agent has an attack animation.");
            character.hasAttackAnimation = EditorGUILayout.Toggle(label, character.hasAttackAnimation);
            label = new GUIContent("Attack Speed", "How many attacks per second the agent will deal");
            character.attacksPerSecond = EditorGUILayout.FloatField(label, character.attacksPerSecond);
            label = new GUIContent("Auto Attack", "");
            character.autoAttack = EditorGUILayout.Toggle(label, character.autoAttack);
            label = new GUIContent("Click To Attack", "If true, the agent will only attack when a valid object is clicked on. Only works if the agent is Player Controlled.");
            character.clickToAttack = EditorGUILayout.Toggle(label, character.clickToAttack);




        }
        private void DrawAnimationGUI()
        {
            /*
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUIContent label = new GUIContent("Under Construction");
            guiStyle.normal.textColor = EditorStyles.label.normal.textColor;
            GUILayout.Label(label, guiStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            */

            GUIContent label;
            label = new GUIContent("Has Animations", "Whether or not this agent has any animations (e.g move, jump, attack...)");
            character.hasAnimations = EditorGUILayout.Toggle(label, character.hasAnimations);

            label = new GUIContent("Animation Type", "Code Based: The agent will directly play the animations within the animator controller, by using their names.\nTransition Based: The agent will use the transitions within the animator controller to change states between different animations.");
            character.animationType = (AnimationType)EditorGUILayout.EnumPopup(label, character.animationType);
            if (character.hasAnimations)
            {
                label = new GUIContent("Animation Transitions", "The names of the animation transitions within the animator controller. Note: only applicable if the Animation type is TransitionBased.");
                GUILayout.Label(label, guiStyle);
                label = new GUIContent("Idle Transition Parameter", "");
                character.idleTransitionParameter = EditorGUILayout.TextField(label, character.idleTransitionParameter);
                label = new GUIContent("Move Transition Parameter", "");
                character.moveTransitionParameter = EditorGUILayout.TextField(label, character.moveTransitionParameter);
                label = new GUIContent("Run Transition Parameter", "");
                character.runTransitionParameter = EditorGUILayout.TextField(label, character.runTransitionParameter);
                label = new GUIContent("Jump Transition Parameter", "");
                character.jumpTransitionParameter = EditorGUILayout.TextField(label, character.jumpTransitionParameter);
                label = new GUIContent("Attack Transition Parameter", "");
                character.attackTransitionParameter = EditorGUILayout.TextField(label, character.attackTransitionParameter);
                label = new GUIContent("Dead Transition Parameter", "");
                character.deadTransitionParameter = EditorGUILayout.TextField(label, character.deadTransitionParameter);
                GUILayout.Space(8);
                label = new GUIContent("Animation Names", "The names of the animations within the animator controller. Note: Only applicable if the Animation type is CodeBased and the Animation Rig Type is Legacy.");
                GUILayout.Label(label, guiStyle);
                label = new GUIContent("Idle Anim Parameter", "");
                character.idleAnimationName = EditorGUILayout.TextField(label, character.idleAnimationName);
                label = new GUIContent("Move Anim Parameter", "");
                character.moveAnimationName = EditorGUILayout.TextField(label, character.moveAnimationName);
                label = new GUIContent("Run Anim Parameter", "");
                character.runAnimationName = EditorGUILayout.TextField(label, character.runAnimationName);
                label = new GUIContent("Jump Anim Parameter", "");
                character.jumpAnimationName = EditorGUILayout.TextField(label, character.jumpAnimationName);
                label = new GUIContent("Attack Anim Parameter", "");
                character.attackAnimationName = EditorGUILayout.TextField(label, character.attackAnimationName);
                label = new GUIContent("Dead Anim Parameter", "");
                character.deadAnimationName = EditorGUILayout.TextField(label, character.deadAnimationName);
                

            }
            
        }
            
        void OnSceneGUI()
        {
            if(character != null)
            {
                //Call the necessary methods to draw the discs and handles on the editor
                if(character.is3D)
                {
                    Color color = Color.cyan;
                    color.a = 0.25f;
                    DrawDiscs(color, character.transform.position, Vector3.up, -character.transform.forward, ref character._detectionRadius, "Detection Radius", float.MaxValue);
                    color = Color.blue;
                    color.a = 0.25f;
                    DrawArcs(color, character.transform.position, Vector3.up, character.transform.forward, character.transform.forward, ref character._lineOfSight, ref character._detectionRadius, "Line of Sight");
                    color = Color.magenta;
                    color.a = 0.15f;
                    DrawDiscs(color, character.transform.position, Vector3.up, -character.transform.right, ref character.statAttackRange.baseValue, "Attack Range", character._detectionRadius);
                    color = new Color(1f, 0f, 0f, 0.75f);//Red
                    DrawArcs(color, character.transform.position, Vector3.up, character.transform.forward, character.transform.right, ref character.statDamageAngle.baseValue, ref character.statAttackRange.baseValue, "Damage Angle");
                    
                }
                else
                {
                    Color color = new Color(1f, 0f, 0f, 0.15f);//Red
                    DrawDiscs(color, character.transform.position, Vector3.forward, character.transform.up, ref character._detectionRadius, "Detection Radius", float.MaxValue);
                    color = new Color(0f, 0f, 1f, 0.35f);//Blue
                    DrawDiscs(color, character.transform.position, Vector3.forward, character.transform.right, ref character.statAttackRange.baseValue, "Attack Range", character._detectionRadius);
                    //color = new Color(1f, 0f, 0f, 0.75f);//Red
                    //DrawArcs(color, character.transform.position, Vector3.forward, character.transform.up, character.transform.up, ref character.m_DamageAngle, ref character.m_AttackRange, "Damage Angle");
                    //color = new Color(0f, 1f, 0f, 0.35f);//Green
                    //DrawArcs(color, character.transform.position, Vector3.forward, character.transform.up, character.transform.forward, ref character._lineOfSight, ref character._detectionRadius, "Line of Sight");
                }
                
            }
            
        }

        protected void DrawDiscs(Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius, float maxValue)
        {
            //
            //Method Name : void DrawDiscs(Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius)
            //Purpose     : This method draws the necessary discs and slider handles in the editor to adjust the attack range and detection radius.
            //Re-use      : none
            //Input       : Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius
            //Output      : none
            //
            //Draw the disc that will represent the detection radius
            Handles.color = color;
            Handles.DrawSolidDisc(center, normal, radius);
            Handles.color = new Color(1f, 1f, 0f, 0.75f);
            Handles.DrawWireDisc(center, normal, radius);

            //Create Slider handles to adjust detection radius properties
            color.a = 0.5f;
            Handles.color = color;
            radius = Handles.ScaleSlider(radius, character.transform.position, direction, Quaternion.identity, radius, 1f);
            radius = Mathf.Clamp(radius, 1f, maxValue);

            

        }
        
        protected void DrawDiscs(Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius, string label, float maxValue)
        {
            //
            //Method Name : void DrawDiscs(Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius, string label)
            //Purpose     : Overloaded method of DrawDiscs(Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius)
            //              that adds the necessary labels. 
            //Re-use      : DrawDiscs(Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius)
            //Input       : Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius, string label
            //Output      : none
            //

            DrawDiscs(color, center, normal, direction, ref radius, maxValue);
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 12;
            labelStyle.normal.textColor = new Color(color.r, color.g, color.b, 1);
            labelStyle.alignment = TextAnchor.UpperCenter;
            Handles.Label(character.transform.position + (direction * radius), label, labelStyle);
        }

        protected void DrawArcs(Color color, Vector3 center, Vector3 normal, Vector3 direction, Vector3 sliderDirection, ref float angle, ref float radius, string label)
        {
            //
            //Method Name : void DrawDiscs(Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius)
            //Purpose     : This method draws the necessary discs and slider handles in the editor to adjust the attack range and detection radius.
            //Re-use      : none
            //Input       : Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius
            //Output      : none
            //
            //Draw the disc that will represent the detection radius
            
            Handles.color = color;
            Vector3 newDirection = character.transform.forward - (character.transform.right);
            Handles.DrawSolidArc(center, normal, direction, angle/2, radius);
            Handles.DrawSolidArc(center, normal, direction, -angle/2, radius);
            Handles.color = new Color(1f, 1f, 0f, 0.75f);
            Handles.DrawWireArc(center, normal, newDirection, angle, radius);

            //Create Slider handles to adjust detection radius properties
            color.a = 0.5f;
            Handles.color = color;
            angle = Handles.ScaleSlider(angle, character.transform.position, sliderDirection, Quaternion.identity, radius, 1f);
            angle = Mathf.Clamp(angle, 1f, 360);

            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 12;
            labelStyle.normal.textColor = new Color(color.r, color.g, color.b, 1);
            labelStyle.alignment = TextAnchor.UpperCenter;
            Handles.Label(character.transform.position + (sliderDirection * radius), label, labelStyle);
        }
        #endregion
    }//end class
}//end namespace

