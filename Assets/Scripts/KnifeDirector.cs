using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDirector : MonoBehaviour
{
    bool isCanCut;//是否切
    Vector3 knifePos;
    Vector3 knifeOldPos;
    [SerializeField] float knifeVelocity = 0.01f;
    Transform knifeTip;
    Vector3 knifeTipPos;
    HandController handController;
    public bool IsCanCut
    {
        get
        {
            return isCanCut;
        }

        set
        {
            isCanCut = value;
        }
    }

    private void Start()
    {
        knifeTip = transform.Find("KnifeTip");
        handController = transform.parent.GetComponentInChildren<HandController>();
    }

    private void OnEnable()
    {
        transform.GetComponentInChildren<BoxCollider>().isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = handController.handVelocity;
        if (Vector3.Angle(velocity.normalized, knifeTip.right.normalized) < 30)
        {
            if (velocity.magnitude > knifeVelocity)
            {
                isCanCut = true;
            }
        }
    }

    /// <summary>
    /// 是否能切砧木
    /// </summary>
    /// <param name="stock"></param>
    /// <returns></returns>
    public bool StockCanCut(Transform stock)
    {
        if (Vector3.Angle(-stock.up, knifeTip.right.normalized) < 30)
            return false;
        else
            return true;
    }
}
