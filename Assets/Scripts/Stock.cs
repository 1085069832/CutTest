using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stock : MonoBehaviour
{
    GrabObject grabObject;
    Transform scion;
    bool isEnter;
    Vector3 scionUp;
    RaycastHit raycastHit;

    void Update()
    {
        if (isEnter)
        {
            if (!grabObject.isGrabing)
            {
                if (Vector3.Angle(scionUp, transform.up) < 30)
                {
                    SetRigidbodyFreeze(scion, RigidbodyConstraints.FreezeAll);
                }
            }
            else
            {
                //重新抓取
                SetRigidbodyFreeze(scion, RigidbodyConstraints.None);
            }
        }

        if (Physics.SphereCast(transform.position, 0.05f, transform.up, out raycastHit))
        {
            print("监测" + raycastHit.point);
        }

    }

    void SetRigidbodyFreeze(Transform objTransf, RigidbodyConstraints rc)
    {
        scion.GetComponent<Rigidbody>().constraints = rc;
        FixedJoint fj = scion.GetComponent<FixedJoint>();
        if (fj)
            fj.connectedBody.GetComponent<Rigidbody>().constraints = rc;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Main")
        {
            scionUp = (collision.transform.GetComponent<Renderer>().bounds.center - collision.contacts[0].point).normalized;
            GetComponent<MeshCollider>().isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Main")
        {
            grabObject = other.GetComponent<GrabObject>();
            scion = other.transform;
            if (grabObject.isGrabing)
                isEnter = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Main")
        {
            print("OnTriggerExit");
            isEnter = false;
            GetComponent<MeshCollider>().isTrigger = false;
        }
    }
}
