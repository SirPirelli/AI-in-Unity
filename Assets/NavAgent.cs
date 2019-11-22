using UnityEngine;
using UnityEngine.AI;

public class NavAgent : MonoBehaviour
{

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        //var agent = GetComponent<NavMeshAgent>().SetDestination(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChaseTarget() { var agent = GetComponent<NavMeshAgent>().SetDestination(target.position); }
}
