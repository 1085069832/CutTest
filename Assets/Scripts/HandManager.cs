using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    LeapProvider leapProvider;
    Frame frame;
    Hand hand;
    [SerializeField] bool isRight;

    private void Awake()
    {
        leapProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    public Hand _hand
    {
        get
        {
            return hand;
        }
    }

    /// <summary>
    /// 获取触碰的手
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Collider")
        {
            frame = leapProvider.CurrentFrame;
            //伸出去的第一只手是0
            if (isRight)
            {
                if (frame.Hands[0].IsRight)
                {
                    hand = frame.Hands[0];
                }
                else
                {
                    hand = frame.Hands[1];
                }
            }
            else
            {
                if (frame.Hands[0].IsLeft)
                {
                    hand = frame.Hands[0];
                }
                else
                {
                    hand = frame.Hands[1];
                }
            }
        }
    }
}
