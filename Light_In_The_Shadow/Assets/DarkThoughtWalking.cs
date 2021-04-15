using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DarkThoughtWalking : DarkThought
{
    private NavMeshAgent _agent;
    private bool _checkingPath;
    public bool shouldAttack = true;

    protected override void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, _playerLayer);
        if (!playerInSightRange) Patrolling();
        if (playerInSightRange && !MasterManager.Instance.player.frozenForCutscene) ChasePlayer();
    }

    private void Patrolling()
    {
        if (!navigationPointSet) SearchWalkPoint();

        if (navigationPointSet)
            _agent.SetDestination(targetNavigationPoint);

        Vector3 distanceToWalkPoint = transform.position - targetNavigationPoint;

        
        if (distanceToWalkPoint.magnitude < 1f)
            navigationPointSet = false;
    }
    private void SearchWalkPoint()
    {
        
        var randomZ = Random.Range(-navigationPointRange, navigationPointRange);
        var randomX = Random.Range(-navigationPointRange, navigationPointRange);

        var position = transform.position;
        var randomPos = new Vector3(position.x + randomX, position.y, position.z + randomZ);
    
        NavMeshHit hit;
        if (!NavMesh.SamplePosition(randomPos, out hit, navigationPointRange, 1)) return;
        targetNavigationPoint = hit.position;
        navigationPointSet = true;
        StartCoroutine(TimeToNextPath());
    }
    
    private void ChasePlayer()
    {
        _agent.SetDestination(player.position);
    }
    
    IEnumerator TimeToNextPath()
    {
        yield return new WaitForSeconds(2.0f);
        if (!_agent.hasPath)
            SearchWalkPoint();
    }
}
