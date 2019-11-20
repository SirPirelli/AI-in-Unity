using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JMPlib;

public class Sight : MonoBehaviour
{

    [SerializeField] private float _radius = 0f;
    [Range(0,360)]
    [SerializeField] private float fovAngle = 0f;
    [SerializeField] private short _rays = 10;
    [SerializeField] private Transform _targetTransform;

    private void Update()
    {
       
    }

    //private void OnDrawGizmos()
    //{

    //    Gizmos.DrawWireSphere(transform.position, _radius);

    //    var distanceToTarget = Vector3.Distance(transform.position, _targetTransform.position);

    //    if (distanceToTarget > _radius) return;

    //    var directionToTarget = (_targetTransform.position - transform.position).normalized;
    //    var angle = Vector3.Angle(transform.forward, directionToTarget);

    //    var fovA = MathExtension.GetDirectionFromAngle(transform.rotation.eulerAngles.y + (fovAngle / 2));
    //    var fovB = MathExtension.GetDirectionFromAngle(transform.rotation.eulerAngles.y + (-fovAngle / 2));

    //    Gizmos.DrawLine(transform.position, transform.position + new Vector3(fovA.x, 0, fovA.y) * _radius);
    //    Gizmos.DrawLine(transform.position, transform.position + new Vector3(fovB.x, 0, fovB.y) * _radius);

    //    Debug.Log("Angle " + angle);
    //    if (angle < fovAngle * 0.5f)
    //    {
            
    //        Gizmos.color = Color.red;

    //        RaycastHit hitInfo = new RaycastHit();
    //        if(Physics.Raycast(transform.position, directionToTarget, out hitInfo, distanceToTarget))
    //        {
    //            if(hitInfo.collider.CompareTag("Enemy"))
    //                Gizmos.color = Color.green;
    //        }

    //        Gizmos.DrawSphere(transform.position, 10f);

    //    }
    //}
}
