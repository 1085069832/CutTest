using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 砧木
/// </summary>
public class Stock : MonoBehaviour
{
    Transform scion;
    bool isEnter;
    RaycastHit raycastHit;
    [SerializeField] Transform tip;
    Vector3 sphereCastDir;
    LayerMask layerMask;
    float radius = 0.02f;
    GrabObject grabObject;

    void Start()
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
                if (!grabObject.isGrabing)
                {
                    if (Vector3.Angle(raycastHit.transform.GetComponent<Renderer>().bounds.center - raycastHit.point, transform.up) < 30)
                    {
                        SetRigidbodyFreeze(raycastHit.transform, RigidbodyConstraints.FreezeAll);
                        isEnter = false;
                    }
                }
            }
        }
    }

    public void SetRigidbodyFreeze(Transform objTransf, RigidbodyConstraints rc)
    {
        objTransf.GetComponent<Rigidbody>().constraints = rc;
        FixedJoint fj = objTransf.GetComponent<FixedJoint>();
        if (fj)
            fj.connectedBody.GetComponent<Rigidbody>().constraints = rc;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Main")
        {
            grabObject = collision.transform.GetComponent<GrabObject>();
            if (grabObject.isGrabing)
            {
                ScionManager scionManager = GameObject.Find("ScionDefPos").GetComponent<ScionManager>();
                scionManager.grabObj = collision.gameObject;
                scionManager.stock = gameObject;
                isEnter = true;
                GetComponent<MeshCollider>().isTrigger = true;
            }
        }
    }
}
