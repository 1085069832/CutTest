using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 砧木
/// </summary>
public class Stock : MonoBehaviour
{
    GrabObject grabObject;
    Transform scion;
    bool isEnter;
    RaycastHit raycastHit;
    [SerializeField] Transform tip;
    Vector3 sphereCastDir;
    LayerMask layerMask;
    float radius = 0.02f;

    private void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Main");
        sphereCastDir = tip.position - transform.position;
    }

    void Update()
    {
        if (isEnter)
        {
            bool isCollider = Physics.SphereCast(transform.position, radius, sphereCastDir, out raycastHit, sphereCastDir.magnitude - radius, layerMask);

            if (isCollider)
            {
                grabObject = raycastHit.transform.GetComponent<GrabObject>();
                if (!grabObject.isGrabing)
                {
                    if (Vector3.Angle(raycastHit.transform.GetComponent<Renderer>().bounds.center - raycastHit.point, transform.up) < 30)
                    {

                        SetRigidbodyFreeze(raycastHit.transform, RigidbodyConstraints.FreezeAll);
                    }
                }
                else
                {
                    //重新抓取
                    SetRigidbodyFreeze(raycastHit.transform, RigidbodyConstraints.None);

                }
            }
            else
            {
                isEnter = false;
            }

        }
    }

    void SetRigidbodyFreeze(Transform objTransf, RigidbodyConstraints rc)
    {
        objTransf.GetComponent<Rigidbody>().constraints = rc;
        FixedJoint fj = objTransf.GetComponent<FixedJoint>();
        if (fj)
            fj.connectedBody.GetComponent<Rigidbody>().constraints = rc;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Main")
        {
            if (other.transform.GetComponent<GrabObject>().isGrabing)
                isEnter = true;
        }
    }
}
