using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class HandController : MonoBehaviour
{
    Hand hand;
    KnifeController knifeController;
    bool isLeftCanGrab;//左手是否能抓取
    bool currentIsGrabing;//当前是否在抓取

    void Awake()
    {
        knifeController = GameObject.Find("GameManager").GetComponent<KnifeController>();
    }

    // Update is called once per frame
    void Update()
    {
        hand = GetComponent<HandManager>()._hand;

        if (hand != null)
        {
            float pinchStrength = hand.PinchStrength;
            float grabAngle = hand.GrabAngle;
            if (hand.IsRight)
            {
                if (pinchStrength > 0.7)
                {
                    knifeController.ShowKnife();
                }
                else
                {
                    if (grabAngle < 2.5)
                    {
                        knifeController.HideKnife();
                    }
                }
            }
            else if (hand.IsLeft)
            {
                if (pinchStrength > 0.7)
                {
                    //GameObject.Find("Cube").transform.position = hand.PalmPosition.ToVector3();
                    isLeftCanGrab = true;
                }
                else
                {
                    if (hand.GrabAngle < 2.5)
                        isLeftCanGrab = false;
                }
            }
        }
    }

    /// <summary>
    /// 拇指尖端
    /// </summary>
    public Vector3 thumbTip
    {
        get
        {
            if (hand != null)
            {
                return hand.Fingers[0].TipPosition.ToVector3();
            }
            else
            {
                return Vector3.zero;
            }
        }
    }

    /// <summary>
    /// 手速度
    /// </summary>
    public Vector3 handVelocity
    {
        get
        {
            return FingersVelocity();
        }
    }

    public bool IsLeftCanGrab
    {
        get
        {
            return isLeftCanGrab;
        }

        set
        {
            isLeftCanGrab = value;
        }
    }

    public bool CurrentIsGrabing
    {
        get
        {
            return currentIsGrabing;
        }

        set
        {
            currentIsGrabing = value;
        }
    }

    Vector3 FingersVelocity()
    {
        if (hand != null)
        {
            Vector3 velocity = Vector3.zero;
            List<Finger> fingers = hand.Fingers;
            foreach (Finger finger in fingers)
            {
                velocity += finger.TipVelocity.ToVector3();
            }
            return (hand.PalmVelocity.ToVector3() + velocity) / 6;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
