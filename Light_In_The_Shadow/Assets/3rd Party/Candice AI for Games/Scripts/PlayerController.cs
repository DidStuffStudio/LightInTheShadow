using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace ViridaxGameStudios.AI
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        public Camera cam;
        GameObject target;
        public GameObject explosionPrefab;
        public GameObject attackProjectile;
        public Transform spawnPosition;
        GameObject attackTarget;
        private NavMeshAgent navMeshAgent;
        public float movementSpeed = 20;
        public float rotationSpeed = 30;
        public bool is3D = true;
        public bool hasAnimations;
        Animator anim;
        public HealthBarScript healthBar;
        float hitPoints = 100;
        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            float translation = (Input.GetAxis("Vertical") * movementSpeed) * Time.deltaTime;
            float rotation = (Input.GetAxis("Horizontal") * rotationSpeed) * Time.deltaTime;

            if (is3D)
            {
                transform.Translate(0, 0, translation);
                transform.Rotate(0, rotation, 0);
            }
            else
            {
                transform.Translate(rotation, translation, 0);
            }

            if (translation == 0)
            {
                if (hasAnimations)
                    anim.SetBool("isRunning", false);
            }
            else
            {
                if (hasAnimations)
                    anim.SetBool("isRunning", true);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    target = hit.transform.gameObject;
                    Debug.Log(target.name);
                    if(target.CompareTag("Enemy"))
                    {
                        attackTarget = target;
                        AttackRange();
                    }
                    

                    //navMeshAgent.SetDestination(hit.point);
                }
            }
            /*
            float translation = (Input.GetAxis("Vertical") * speed) * Time.deltaTime;
            float rotation = (Input.GetAxis("Horizontal") * rotationSpeed) * Time.deltaTime;
            transform.Translate(0, 0, translation);
            transform.Rotate(0, rotation, 0);
            */

        }
        public void AttackRange()
        {
            //
            //Method Name : void AttackRange()
            //Purpose     : This method is called by the attack animation event. Deals the required damage to all targets in range..
            //Re-use      : none
            //Input       : none
            //Output      : none
            //
            GameObject projectile = Instantiate(attackProjectile, spawnPosition.position, Quaternion.identity);

            //arrow.transform.position = spawnPosition.position;
            SimpleAIController ai = projectile.GetComponent<SimpleAIController>();
            ai.target = attackTarget;
            ai.Fire(gameObject);
        }
        public void ReceiveDamage(float damage)
        {
            if(healthBar != null)
            {
                hitPoints -= damage;
                healthBar.SetHealth(hitPoints);
                if(hitPoints <= 0)
                {

                }
            }
        }
    }

}
