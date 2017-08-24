using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutObject : MonoBehaviour
{
    Vector3 enterPos;
    GameObject knife;
    KnifeDirector knifeDirector;
    bool isCollider;
    GameObject[] newGameObjects;
    [SerializeField] Rigidbody rg;
    [SerializeField] HingeJoint hj;

    /// <summary>
    /// 截取
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="plane"></param>
    private void DoCut(GameObject obj, Plane plane)
    {
        obj.GetComponent<ShatterTool>().Split(new Plane[] { plane }, out newGameObjects);
        if (newGameObjects.Length > 1)
        {
            print("size1 " + newGameObjects[0].GetComponent<MeshCollider>().bounds.size.magnitude);
            print("size2 " + newGameObjects[1].GetComponent<MeshCollider>().bounds.size);
            if (newGameObjects[0].GetComponent<MeshCollider>().bounds.size.magnitude > newGameObjects[1].GetComponent<MeshCollider>().bounds.size.magnitude)
            {
                Destroy(newGameObjects[1]);
            }
            else
            {
                Destroy(newGameObjects[0]);
            }
            newGameObjects = null;
        }
    }

    private void AddJoint()
    {
        foreach (GameObject newObject in newGameObjects)
        {
            FixedJoint fj = newObject.AddComponent<FixedJoint>();
            fj.connectedBody = rg;
        }
    }

    private void FixedUpdate()
    {
        print("1" + isCollider);
        if (isCollider && knifeDirector.IsCanCut)
        {
            Plane plane = new Plane(knife.transform.up, enterPos);
            DoCut(gameObject, plane);
            knifeDirector.IsCanCut = false;
            isCollider = false;
            knife.GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "KnifeBlade")
        {
            knife = collision.gameObject;
            knifeDirector = knife.GetComponentInParent<KnifeDirector>();
            enterPos = collision.contacts[0].point;
            knife.transform.GetComponent<BoxCollider>().isTrigger = true;
            isCollider = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.name == "KnifeBlade")
        {
            isCollider = false;
            collision.transform.GetComponent<BoxCollider>().isTrigger = false;
        }
    }
}
