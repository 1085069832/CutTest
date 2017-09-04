using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    float force = 50;
    bool canGrab;
    HandController handController;
    FixedJoint fj;
    Transform handCenter;
    [HideInInspector] public bool isGrabing;

    void Update()
    {
        if (handController)
        {
            if (canGrab && handController.IsLeftCanGrab)
            {
                //抓取
                DoGrab(true, handController);
            }

            if (!handController.IsLeftCanGrab)
            {
                //断开
                DoGrab(false, handController);
            }
        }

    }

    /// <summary>
    /// 抓取和丢弃
    /// </summary>
    /// <param name="isGrab"></param>
    /// <param name="handController"></param>
    void DoGrab(bool isGrab, HandController handController)
    {
        if (isGrab)
        {
            if (!fj)
            {
                fj = gameObject.AddComponent<FixedJoint>();
                fj.connectedBody = handCenter.gameObject.GetComponent<Rigidbody>();
                isGrabing = true;
                SetGrabObjTrigger(true);
                handController.CurrentIsGrabing = true;
            }
        }
        else
        {
            if (fj)
            {
                Destroy(fj);
                isGrabing = false;
                canGrab = false;
                handController.IsLeftCanGrab = false;
                handController.CurrentIsGrabing = false;
                SetGrabObjTrigger(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        RigidFinger rf = other.GetComponentInParent<RigidFinger>();
        RigidHand rh = other.GetComponentInParent<RigidHand>();

        if (rf && rh)
        {
            handCenter = rh.transform.Find("palm").Find("HandCenter");
            handController = rh.GetComponentInChildren<HandController>();

            if (handController.IsLeftCanGrab && !handController.CurrentIsGrabing)
            {
                if (rf && (rf.fingerType == Leap.Finger.FingerType.TYPE_THUMB || rf.fingerType == Leap.Finger.FingerType.TYPE_INDEX || rf.fingerType == Leap.Finger.FingerType.TYPE_MIDDLE))
                {
                    if (other.name == "bone3")
                    {
                        canGrab = true;
                    }
                }
            }

            if (!canGrab)
                AddForceForHand(handController.handVelocity);
        }
    }

    /// <summary>
    /// 添加力
    /// </summary>
    /// <param name="handVelocity"></param>
    void AddForceForHand(Vector3 handVelocity)
    {
        GetComponent<Rigidbody>().AddForce(handVelocity * GetComponent<Rigidbody>().mass * force, ForceMode.Force);
    }

    /// <summary>
    /// 抓取后obj trigger
    /// </summary>
    /// <param name="isTrigger"></param>
    void SetGrabObjTrigger(bool isTrigger)
    {
        GetComponent<MeshCollider>().isTrigger = isTrigger;
        var fj = GetComponent<FixedJoint>();
        if (fj)
        {
            var meshCollider = fj.connectedBody.GetComponent<MeshCollider>();
            if (meshCollider)
                meshCollider.isTrigger = isTrigger;
        }
    }
}
