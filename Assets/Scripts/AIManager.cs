using AI.DecisionTree;
using JMPlib;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    [SerializeField] private Transform target = null;
    [SerializeField] private List<GameObject> enemies = null;
    [Range(0, 500)]
    [SerializeField] private float viewDistance = 50f;
    [Range(0, 360)]
    [SerializeField] private float fov = 90f;
    [SerializeField] private float updateTimeStep = 0.2f;

    Node positive = new ActionNode(() => Debug.Log("Enemy in sight!"));
    Node negative = new ActionNode(() => Debug.Log("Enemy out of sight!"));

    Coroutine aiCoroutine;

    Dictionary<GameObject, Node> enemiesDecisionTree;

    // Start is called before the first frame update
    void Start()
    {

        enemiesDecisionTree = new Dictionary<GameObject, Node>();

        foreach (var enemy in enemies)
        {
            //initialize conditions
            ArgCondition<Transform> viewDistanceNode = new ArgCondition<Transform>(IsInViewDistance, enemy.transform);
            ArgCondition<Transform> coneOfViewNode = new ArgCondition<Transform>(IsInConeOfView, enemy.transform);
            ArgCondition<Transform> clearSightNode = new ArgCondition<Transform>(IsInClearSight, enemy.transform);

            //initialize nodes
            Node thirdLayer = new BinaryDecisionNode(positive, negative, clearSightNode);
            Node secondLayer = new BinaryDecisionNode(thirdLayer, negative, coneOfViewNode);
            Node root = new BinaryDecisionNode(secondLayer, negative, viewDistanceNode);

            enemiesDecisionTree.Add(enemy, root);

        }
        
        aiCoroutine = StartCoroutine(AIFrame());
    }

    private void OnDrawGizmos()
    {
        foreach (var item in enemies)
        {
            Handles.DrawWireArc(item.transform.position, item.transform.up, item.transform.forward, 360, viewDistance);
            Handles.color = Color.yellow;
            var fovA = MathExtension.GetDirectionFromAngle(item.transform.eulerAngles.y + (-fov / 2));
            var fovB = MathExtension.GetDirectionFromAngle(item.transform.eulerAngles.y + fov / 2);
            Handles.DrawLine(item.transform.position, item.transform.position + new Vector3(fovA.x, 0, fovA.y) * viewDistance);
            Handles.DrawLine(item.transform.position, item.transform.position + new Vector3(fovB.x, 0, fovB.y) * viewDistance);

        }
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
                item.Value.Eval();

            yield return new WaitForSecondsRealtime(updateTimeStep);
        }
        
    }
}
