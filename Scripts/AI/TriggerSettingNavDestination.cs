using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.AI;

public class TriggerSettingNavDestination : MonoBehaviour
{
    private PathFollowingController pathFollowingcontroller;
    private NavMeshAgent navMeshAgent;

    public PathCreator pathCreator;
    public GameObject character;

    void Start()
    {
        pathFollowingcontroller = character.GetComponent<PathFollowingController>();
        navMeshAgent = character.GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            pathFollowingcontroller.Set_pathCreator(pathCreator);

            navMeshAgent.enabled = true;
            pathFollowingcontroller.SetNavDestination(true);
        }
    }
}
