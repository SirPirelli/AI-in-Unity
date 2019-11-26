using UnityEngine;
using UnityEngine.AI;

public class NavAgent : MonoBehaviour
{

    public Transform target;
    [HideInInspector] public NavMeshAgent navMeshAgent;

    public bool IsChasingTarget { get; set; }

    private Vector3 destination;

    // Start is called before the first frame update
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        IsChasingTarget = false;
    }


    public void ChaseTarget()
    {
        navMeshAgent.SetDestination(target.position);
        IsChasingTarget = true;

        destination = target.position;
    }

    public bool SetDestination(Vector3 destination)
    {
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(destination, out navMeshHit, 1f, 1)) //view distnce should be passed by reference
        {
            this.destination = navMeshHit.position;
            navMeshAgent.SetDestination(this.destination);
            return true;
        }

        return false;
       

    }

    public bool HasReachedDestination()
    {
        //NavMeshHit navMeshHit;
        //var b = navMeshAgent.SamplePathPosition(-1, navMeshAgent.radius, out navMeshHit);

        return Mathf.Abs(navMeshAgent.remainingDistance) <= navMeshAgent.radius ? true : false;        

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if(destination != null)
            Gizmos.DrawCube(destination, Vector3.one * 20f);
    }

}
