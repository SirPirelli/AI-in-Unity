using AI.DecisionTree;
using JMPlib;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{

    [SerializeField] private Transform target = null;
    [SerializeField] private List<GameObject> enemies = null;
    [Range(0, 300)][SerializeField] private float speed = 0f;
    [Range(0, 300)] [SerializeField] private float acceleration = 0f;
    [Range(0, 300)] [SerializeField] private float angularSpeed = 0f;
    [Range(0, 300)] [SerializeField] private float stoppingDistance = 0f;
    [Range(0, 500)][SerializeField] private float viewDistance = 50f;
    [Range(0, 360)][SerializeField] private float fov = 90f;

    [SerializeField] private float updateTimeStep = 0.2f;

    Node positive = new ActionNode(() => Debug.Log("Enemy in sight!"));
    Node negative = new ActionNode(() => Debug.Log("Enemy out of sight!"));
    //Node setTarget = new ArgAction<>

    Coroutine aiCoroutine;

    Dictionary<GameObject, Node> enemiesDecisionTree;

    private void OnValidate()
    {
        if(enemiesDecisionTree != null)
        foreach (var item in enemiesDecisionTree)
        {
            var agent = item.Key.GetComponent<NavAgent>();
            agent.navMeshAgent.speed = speed;
            agent.navMeshAgent.acceleration = acceleration;
            agent.navMeshAgent.angularSpeed = angularSpeed;
            agent.navMeshAgent.stoppingDistance = stoppingDistance;

        }
    }

    // Start is called before the first frame update
    void Start()
    {

        enemiesDecisionTree = new Dictionary<GameObject, Node>();

        foreach (var enemy in enemies)
        {
            var navAgent = enemy.GetComponent<NavAgent>();
            navAgent.navMeshAgent.speed = speed;
            navAgent.navMeshAgent.angularSpeed = angularSpeed;
            navAgent.navMeshAgent.acceleration = acceleration;
            navAgent.navMeshAgent.stoppingDistance = stoppingDistance;

            //initialize conditions
            ArgCondition<NavAgent> hasReachedDestination = new ArgCondition<NavAgent>(HasReachedDestination, navAgent);
            ArgCondition<Transform> viewDistanceNode = new ArgCondition<Transform>(IsInViewDistance, enemy.transform);
            ArgCondition<Transform> coneOfViewNode = new ArgCondition<Transform>(IsInConeOfView, enemy.transform);
            ArgCondition<Transform> clearSightNode = new ArgCondition<Transform>(IsInClearSight, enemy.transform);

            ArgAction<NavAgent> startChaseAction = new ArgAction<NavAgent>(StartChase, navAgent);
            ArgAction<NavAgent> setRandomTarget = new ArgAction<NavAgent>(SetRandomTarget, navAgent);

            //initialize nodes
            Node hasReachedDestNode = new BinaryDecisionNode(setRandomTarget, negative, hasReachedDestination);
            Node isInSight = new BinaryDecisionNode(startChaseAction, hasReachedDestNode, clearSightNode);
            Node isInConeOfView = new BinaryDecisionNode(isInSight, hasReachedDestNode, coneOfViewNode);
            Node root = new BinaryDecisionNode(isInConeOfView, hasReachedDestNode, viewDistanceNode);

            enemiesDecisionTree.Add(enemy, root);

        }
        
        aiCoroutine = StartCoroutine(AIFrame());
    }

    private void OnDrawGizmos()
    {
        foreach (var item in enemies)
        {
            Handles.color = Color.white;
            Handles.DrawWireArc(item.transform.position, item.transform.up, item.transform.forward, 360, viewDistance);
            Handles.color = Color.yellow;
            var fovA = MathExtension.GetDirectionFromAngle(item.transform.eulerAngles.y + (-fov / 2));
            var fovB = MathExtension.GetDirectionFromAngle(item.transform.eulerAngles.y + fov / 2);
            Handles.DrawLine(item.transform.position, item.transform.position + new Vector3(fovA.x, 0, fovA.y) * viewDistance);
            Handles.DrawLine(item.transform.position, item.transform.position + new Vector3(fovB.x, 0, fovB.y) * viewDistance);

        }
    }

    private bool IsChasingTarget(NavAgent navAgent)
    {
        return navAgent.IsChasingTarget;
    }

    private void SetRandomTarget(NavAgent navAgent)
    {

        Vector2 point = Vector2.zero;
        Vector3 destination = Vector3.zero;

        do
        {

            point = Random.insideUnitCircle;
            destination = new Vector3(point.x, 0, point.y) * viewDistance;

        } while (!navAgent.SetDestination(destination));

    }

    private void StartChase(NavAgent navAgent)
    {
        navAgent.ChaseTarget();
    }

    private bool HasReachedDestination(NavAgent navAgent)
    {
        return navAgent.HasReachedDestination();
    }

    private bool IsInViewDistance(Transform transform)
    {

        var distance = Vector3.Distance(target.position, transform.position);

        return distance < viewDistance ? true : false;
        
    }

    private bool IsInConeOfView(Transform transform)
    {
        var directionToTarget = (target.position - transform.position).normalized;
        var angle = Vector3.Angle(transform.forward, directionToTarget);

        return angle < fov * 0.5f ? true : false;
    }

    private bool IsInClearSight(Transform transform)
    {
        var direction = (target.transform.position - transform.position).normalized;

        RaycastHit raycastHit;
        if(Physics.Raycast(transform.position, direction, out raycastHit))
        {
            return raycastHit.collider.gameObject == target.gameObject;
        }

        return false;
    }

    IEnumerator AIFrame()
    {

        while(true)
        {
            foreach (var item in enemiesDecisionTree)
            {
                item.Value.Eval();
                yield return null;
            }

            yield return new WaitForSecondsRealtime(updateTimeStep);
        }
        
    }
}
