using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViridaxGameStudios.AI;

namespace ViridaxGameStudios
{
    public class TacticsPlayer : MonoBehaviour
    {
        public string playerName;
        public List<CandiceAIController> units;
        private CandiceAIController selectedUnit;
        public bool turn = false;

        void Start()
        {
            CandiceAIManager.getInstance().OnDestinationReached += onDestinationReached;
            foreach (CandiceAIController agent in units)
            {
                agent._tacticsPlayer = this;
            }
        }
        void Update()
        {
            if (turn)
            {
                CheckClicked();
            }

            /*
            if(selectedUnit != null)
            {
                PlayerMove playerMove = selectedUnit.GetComponent<PlayerMove>();
                if(playerMove != null)
                {
                    playerMove.enabled = true;
                    playerMove.turn = true;
                }
            }
            */
        }

        public bool IsAlly(CandiceAIController agent)
        {
            bool isAlly = false;
            int count = 0;
            while (!isAlly && count < units.Count)
            {
                if (units[count]._agentID == agent._agentID)
                {
                    isAlly = true;
                }
                count++;
            }

            return isAlly;
        }
        void onDestinationReached(CandiceAIController agent)
        {
            foreach (CandiceAIController unit in units)
            {
                if (agent._agentID == unit._agentID)
                {
                    EndTurn();
                }
            }
        }
        public void BeginTurn()
        {
            Debug.Log("Begiining " + playerName + " turn");
            turn = true;
        }

        public void EndTurn()
        {
            ClearSelectedUnit();
            turn = false;
            TurnManager.EndTurn(this);
        }

        void CheckClicked()
        {
            if (Input.GetMouseButtonUp(0))
            {
                //Debug.Log("Clicked");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject go = hit.collider.gameObject;
                    CandiceAIController agent = go.GetComponent<CandiceAIController>();
                    if (agent != null)
                    {
                        foreach (CandiceAIController ai in units)
                        {
                            if (agent._agentID == ai._agentID)
                            {
                                CandiceAIManager.getInstance().ComputeAdjacencyList(agent.jumpHeight);
                                Debug.Log("Clicked agent " + agent._agentID);
                                if (selectedUnit != null)
                                {
                                    ClearSelectedUnit();
                                }
                                selectedUnit = agent;
                                selectedUnit.isSelected = true;
                                selectedUnit.turn = true;
                                selectedUnit.isPlayerControlled = true;
                            }
                        }
                    }
                }
            }
        }

        void ClearSelectedUnit()
        {
            selectedUnit.isSelected = false;
            selectedUnit.turn = false;
            selectedUnit.RemoveSelectableTiles();
            selectedUnit.isPlayerControlled = false;
        }
    }

}
