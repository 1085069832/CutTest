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
    LeafManager leafManager;
    MainManager mainManager;

    private void Awake()
    {
        var gameManager = GameObject.Find("GameManager");
        leafManager = gameManager.GetComponent<LeafManager>();
        mainManager = gameManager.GetComponent<MainManager>();

        SaveLeafGo();
    }

    void SaveLeafGo()
    {
        if (transform.parent)
        {
            if (transform.parent.tag == "Leaf")
            {
                leafManager.newGameObjectsLists.Add(transform.parent.gameObject);
                transform.parent = null;
            }
        }
    }

    /// <summary>
    /// 截取
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="plane"></param>
    private void DoCut(GameObject obj, Plane plane)
    {
        obj.GetComponent<ShatterTool>().Split(new Plane[] { plane }, out newGameObjects);
        AddNewObjsToList(newGameObjects);
        //leafManager.newObjects = newGameObjects;
        if (newGameObjects.Length > 1)
        {
            if (transform.tag == "Leaf")
            {
                if (newGameObjects[0].GetComponent<MeshCollider>().bounds.size.magnitude > newGameObjects[1].GetComponent<MeshCollider>().bounds.size.magnitude)
                {
                    newGameObjects[1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    GetOrAddComponent<Rigidbody>(newGameObjects[1]).useGravity = true;
                }
                else
                {
                    newGameObjects[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    GetOrAddComponent<Rigidbody>(newGameObjects[0]).useGravity = true;
                }
            }
            else if (transform.tag == "Main")
            {
                //if ((newGameObjects[0].transform.position - mainManager.growTransf.position).magnitude > (newGameObjects[1].transform.position - mainManager.growTransf.position).magnitude)
                //{
                //    newGameObjects[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                //    GetOrAddComponent<Rigidbody>(newGameObjects[0]).useGravity = true;
                //}
                //else
                //{
                //    newGameObjects[1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                //    GetOrAddComponent<Rigidbody>(newGameObjects[1]).useGravity = true;
                //}
            }
        }
        newGameObjects = null;
    }

    /// <summary>
    /// 添加新物体到list
    /// </summary>
    /// <param name="newGameObjects"></param>
    void AddNewObjsToList(GameObject[] newGameObjects)
    {
        for (int i = 0; i < newGameObjects.Length; i++)
        {
            if (newGameObjects[i].tag == "Leaf")//叶子
                leafManager.newGameObjectsLists.Add(newGameObjects[i]);
            else if (newGameObjects[i].tag == "Main")
                mainManager.newGameObjectsLists.Add(newGameObjects[i]);
        }
    }

    T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        if (go.GetComponent<T>())
        {
            return go.GetComponent<T>();
        }
        else
        {
            return go.AddComponent<T>();
        }
    }

    private void Update()
    {
        if (isCollider)
        {
            if (knifeDirector.IsCanCut)
            {
                Plane plane = new Plane(knife.transform.up, enterPos);
                DoCut(gameObject, plane);
                knifeDirector.IsCanCut = false;
                knife.transform.GetComponent<BoxCollider>().isTrigger = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isCollider && collision.transform.name == "KnifeBlade")
        {
            knife = collision.gameObject;
            knife.transform.GetComponent<BoxCollider>().isTrigger = true;
            knifeDirector = knife.GetComponentInParent<KnifeDirector>();
            enterPos = collision.contacts[0].point;
            if (transform.tag == "Main")
                mainManager.EnterPos = enterPos;
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
