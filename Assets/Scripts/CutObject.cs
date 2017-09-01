using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Hy
{
    Count0,
    Count1,
    Count2,
    Count3,
    Count4,
    Count5
}

public class CutObject : MonoBehaviour
{
    Vector3 enterPos;
    GameObject knife;
    KnifeDirector knifeDirector;
    bool isCollider;
    GameObject[] newGameObjects;
    LeafManager leafManager;
    MainManager mainManager;
    public Hy hy;

    private void Awake()
    {

        FindPlantManager();

        SaveLeafGo();
    }

    void FindPlantManager()
    {
        GameObject plantManager = null;

        switch (hy)
        {
            case Hy.Count0:
                plantManager = GameObject.Find("PlantManagers/PlantManager0");
                break;
            case Hy.Count1:
                plantManager = GameObject.Find("PlantManagers/PlantManager1");
                break;
            case Hy.Count2:
                plantManager = GameObject.Find("PlantManagers/PlantManager2");
                break;
            case Hy.Count3:
                plantManager = GameObject.Find("PlantManagers/PlantManager3");
                break;
            case Hy.Count4:
                plantManager = GameObject.Find("PlantManagers/PlantManager4");
                break;
            case Hy.Count5:
                plantManager = GameObject.Find("PlantManagers/PlantManager5");
                break;
        }

        leafManager = plantManager.GetComponent<LeafManager>();
        mainManager = plantManager.GetComponent<MainManager>();
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
        if (newGameObjects.Length > 1)
        {
            if (transform.tag == "Leaf")
            {
                if (newGameObjects[0].GetComponent<MeshCollider>().bounds.size.magnitude > newGameObjects[1].GetComponent<MeshCollider>().bounds.size.magnitude)
                {
                    SetGoValue(newGameObjects[1]);
                }
                else
                {
                    SetGoValue(newGameObjects[0]);
                }
            }
            else if (transform.tag == "Main")
            {
                if ((newGameObjects[0].GetComponent<Renderer>().bounds.center - mainManager.growTransf.position).magnitude > (newGameObjects[1].GetComponent<Renderer>().bounds.center - mainManager.growTransf.position).magnitude)
                {
                    SetGoValue(newGameObjects[0]);
                    if (leafManager.raycastHit.transform)
                    {
                        newGameObjects[0].AddComponent<FixedJoint>().connectedBody = leafManager.raycastHit.transform.GetComponent<Rigidbody>();
                    }
                }
                else
                {
                    SetGoValue(newGameObjects[1]);
                    if (leafManager.raycastHit.transform)
                    {
                        newGameObjects[1].AddComponent<FixedJoint>().connectedBody = leafManager.raycastHit.transform.GetComponent<Rigidbody>();
                    }
                }
            }
        }
    }

    void SetGoValue(GameObject go)
    {
        go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetOrAddComponent<Rigidbody>(go).useGravity = true;
        go.GetComponent<CutObject>().enabled = false;
        go.GetComponent<ShatterTool>().enabled = false;
        go.GetComponent<TargetUvMapper>().enabled = false;
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
