using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> newGameObjectsLists = new List<GameObject>();
    LeafManager leafManager;
    public Transform growTransf;
    [SerializeField] GameObject test;
    Vector3 enterPos;//刀切点
    GameObject minGo;

    public Vector3 EnterPos
    {
        set
        {
            enterPos = value;
        }
    }

    private void Awake()
    {
        leafManager = GetComponent<LeafManager>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (newGameObjectsLists.Count > 0)
        {
            leafManager.OnLeafDrop();
        }

        //if (test)
        //    print(test.GetComponent<MeshCollider>().bounds.size.magnitude);

        //if (enterPos != Vector3.zero)
        //{
        //    print("切距离" + (enterPos - growTransf.position).magnitude);

        //}
    }
}
