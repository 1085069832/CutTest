using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    float force = 40;
    bool canGrab;
    HandController handController;
    bool isEnter;
    FixedJoint fj;
    Transform handCenter;
    [HideInInspector] public bool isGrabing;

    void Update()
    {
        if (canGrab && handController.IsLeftCanGrab)
        {
            //抓取
            if (!fj)
            {
                fj = gameObject.AddComponent<FixedJoint>();
                fj.connectedBody = handCenter.gameObject.GetComponent<Rigidbody>();
                isGrabing = true;
            }
        }

        if (isEnter && !handController.IsLeftCanGrab)
        {
            //断开
            if (fj)
            {
                Destroy(fj);
                isEnter = false;
                isGrabing = false;
                canGrab = false;
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
            isEnter = true;
            if (rf && rf.fingerType == Leap.Finger.FingerType.TYPE_THUMB)
            {
                //是否是拇指
                canGrab = true;
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
}
