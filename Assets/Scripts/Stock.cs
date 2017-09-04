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
    RaycastHit raycastHitMain;
    RaycastHit raycastHitStock;
    [SerializeField] Transform tip;
    Vector3 sphereCastDir;
    LayerMask mainLayerMask;
    LayerMask stockLayerMask;
    float radius = 0.02f;//stock粗细
    GrabObject grabObject;
    bool isGrafting;
    StockManager stockManager;

    void Awake()
    {
        mainLayerMask = 1 << LayerMask.NameToLayer("Main");
        stockLayerMask = 1 << LayerMask.NameToLayer("Stock");
        stockManager = GameObject.Find("StockManager").GetComponent<StockManager>();
    }

    void Start()
    {
        sphereCastDir = tip.position - transform.position;
    }

    void Update()
    {
        if (stockManager.isCutStock)
        {
            bool isMain = Physics.SphereCast(transform.position, radius, sphereCastDir, out raycastHitMain, sphereCastDir.magnitude - radius, mainLayerMask);

            if (isMain)
            {
                if (!isGrafting)
                {
                    GetComponent<MeshCollider>().isTrigger = true;
                    var main = raycastHitMain.transform;
                    var mainCenter = main.GetComponent<Renderer>().bounds.center;
                    var dir = raycastHitMain.point - mainCenter;
                    Ray ray = new Ray(mainCenter, dir);
                    bool isStock = Physics.Raycast(ray, out raycastHitStock, dir.magnitude - 0.01f, stockLayerMask);

                    if (isStock)
                    {
                        if (Vector3.Angle(raycastHitMain.transform.GetComponent<Renderer>().bounds.center - raycastHitMain.point, transform.up) < 30)
                        {
                            grabObject = raycastHitMain.transform.GetComponent<GrabObject>();
                            if (!grabObject.isGrabing)
                            {
                                if (isEnter)
                                {
                                    SetRigidbodyFreeze(raycastHitMain.transform, RigidbodyConstraints.FreezeAll);
                                    isGrafting = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        isEnter = false;
                    }
                }
            }
            else
            {
                GetComponent<MeshCollider>().isTrigger = false;
            }

            if (isMain && isGrafting)
            {
                if (grabObject.isGrabing)//重新抓取
                {
                    SetRigidbodyFreeze(raycastHitMain.transform, RigidbodyConstraints.None);
                    isGrafting = false;
                }
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
            if (other.GetComponent<GrabObject>().isGrabing)
            {
                isEnter = true;
            }
        }
    }
}
