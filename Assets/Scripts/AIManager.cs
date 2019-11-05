using AI.DecisionTree;
using System.Collections;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    [SerializeField] private GameObject player = null;
    [SerializeField] private GameObject enemy = null;
    [SerializeField] private float sightRadius = 50f;
    [SerializeField] private float sightEndRadius = 10f;
    [SerializeField] private float updateTimeStep = 0.2f;

    Node positive = new ActionNode(() => Debug.Log("Enemy in sight!"));
    Node negative = new ActionNode(() => Debug.Log("Enemy out of sight!"));
    Node root;

    Coroutine aiCoroutine;
    bool hit = false;
    Vector3 gizmoPos = Vector3.zero;
    float rayDistance = 0f;

    // Start is called before the first frame update
    void Start()
    {
        root = new BinaryDecisionNode(positive, negative, IsOnSight);
        aiCoroutine = StartCoroutine(AIFrame());
    }

    private void OnDrawGizmos()
    {

        var dir = (enemy.transform.position - player.transform.position);
        dir.Normalize();

        if (hit) Gizmos.color = Color.green;
        else Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(gizmoPos, sightEndRadius);
        Gizmos.DrawRay(player.transform.position, dir * rayDistance);
    }


    private bool IsOnSight()
    {

        var direction = enemy.transform.position - player.transform.position;
        direction.Normalize();

        RaycastHit raycastHit;

        if (Physics.SphereCast(player.transform.position, sightEndRadius, direction, out raycastHit, sightRadius))
        {
            gizmoPos = raycastHit.point;
            rayDistance = raycastHit.distance;

            if (raycastHit.transform.gameObject == enemy)
            {
                hit = true;
            }
            else
            {
                hit = false;
            }
        }
        else
        {
            gizmoPos = player.transform.position + direction * sightRadius;
            rayDistance = sightRadius;
            hit = false;
        }

        return hit;
        
    }

    IEnumerator AIFrame()
    {

        while(true)
        {
            root.Eval();

            yield return new WaitForSecondsRealtime(updateTimeStep);
        }
        
    }
}
