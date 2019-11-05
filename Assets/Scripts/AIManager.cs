using AI.DecisionTree;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float sightRadius = 50f;

    Node positive = new ActionNode(() => Debug.Log("Enemy in sight!"));
    Node negative = new ActionNode(() => Debug.Log("Enemy out of sight!"));
    Node root;
    float timer = 0f;

    Coroutine aiCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        root = new BinaryDecisionNode(positive, negative, CheckDistance);
        aiCoroutine = StartCoroutine(AIFrame());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(player.transform.position, sightRadius);
    }

    private bool CheckDistance()
    {
        float distance = Mathf.Abs(Vector3.Distance(player.transform.position, enemy.transform.position));
        if (distance <= sightRadius) return true;

        return false;
    }

    IEnumerator AIFrame()
    {

        while(true)
        {
            root.Eval();

            yield return new WaitForSecondsRealtime(0.2f);
        }
        
    }
}
