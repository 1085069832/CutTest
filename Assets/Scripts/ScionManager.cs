using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScionManager : MonoBehaviour
{

    [HideInInspector] public GameObject grabObj;
    [HideInInspector] public GameObject stock;

    public void ResetScion()
    {
        if (grabObj)
        {
            stock.GetComponent<Stock>().SetRigidbodyFreeze(grabObj.transform,RigidbodyConstraints.None);
            //grabObj.transform.position = transform.position;
            stock.GetComponent<MeshCollider>().isTrigger = false;
        }
    }
}
