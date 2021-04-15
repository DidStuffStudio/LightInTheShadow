using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlyingBeanSpider : DarkThought
{
    public bool _redirecting;
    [SerializeField] private Collider roamingVolume;
    private bool _isAttacking;
    [SerializeField] private float flyingSpeed = 10.0f, rotationSpeed = 5.0f;
    [SerializeField] private Transform flyTowardsPoint;
    [SerializeField] private float timeBetweenAttacks, fallbackDestroyTime = 30.0f;
    private bool _canAttack = true, _hasReachedFirstPoint;
    private bool hasAttackedInLastXSeconds;

    protected override void Start()
    {
        roamingVolume = GameObject.FindWithTag("FlyingBounds").GetComponent<Collider>();
        
        base.Start();
        GetRandomPointInRange();
        StartCoroutine(FallbackDestroy());
    }

    protected override void FixedUpdate()
    
        {
            base.FixedUpdate();
            if (collided)
            {
                print("Collided with player");
                hasAttackedInLastXSeconds = true;
                GetRandomPointInRange();
                StartCoroutine(WaitToAttack());
            }
            
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, _playerLayer);
            
            if (playerInSightRange && _canAttack && _hasReachedFirstPoint)
            {
                _isAttacking = true;
                targetNavigationPoint = MasterManager.Instance.player.transform.position;
            }
            else
            {
                _isAttacking = false;
            }

            Movement();
    }
    
    void Movement()
    {
        if(!navigationPointSet && !_isAttacking) GetRandomPointInRange();
        var position = transform.position;
        
        //Change fly toward point
        flyTowardsPoint.position = Vector3.MoveTowards(flyTowardsPoint.position, targetNavigationPoint, Time.deltaTime*flyingSpeed);
        transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( flyTowardsPoint.position - position), Time.deltaTime*rotationSpeed);
        transform.position = Vector3.MoveTowards(position, flyTowardsPoint.position, Time.deltaTime*flyingSpeed);
        
        var distanceToTarget = flyTowardsPoint.position - targetNavigationPoint;
        
        if (distanceToTarget.magnitude < 1f && !_isAttacking)
        {
            navigationPointSet = false;
            _hasReachedFirstPoint = true;
            _canAttack = true;
        }

        else if(distanceToTarget.magnitude < 1f) 
        {
            _canAttack = false;
        }
        /*if(_isAttacking && distanceToTarget.magnitude < 1f)
        {
            targetNavigationPoint = player.transform.position + new Vector3(0,1.5f,0);
        }*/
            

    }
    
    

    public void GetRandomPointInRange()
    {
        if (navigationPointSet) return;
        var bounds = roamingVolume.bounds;
        var point = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z));
        /*if(Physics.Raycast(point, point - transform.position, Mathf.Infinity, ~LayerMask.NameToLayer("Player")))
        {
            print("Getting another point");
            GetRandomPointInRange();
            return;
        }*/
        targetNavigationPoint = point;
        navigationPointSet = true;
    }

    IEnumerator WaitToAttack()
    {
        _isAttacking = false;
        _canAttack = false;
        yield return new WaitForSeconds(timeBetweenAttacks);
        _canAttack = true;

    }

    IEnumerator FallbackDestroy() //If hasn't managed to attack in x seconds destroy it so it doesn't break the game
    {
        while (alive)
        {
            yield return new WaitForSeconds(fallbackDestroyTime);
            if(!hasAttackedInLastXSeconds)StartCoroutine(Explode());
            hasAttackedInLastXSeconds = false;
        }
    }
}
