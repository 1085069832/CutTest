using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> newGameObjectsLists = new List<GameObject>();
    LeafManager leafManager;
    public Transform growTransf;
    Vector3 enterPos;//刀切点

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

    // Update is called once per frame
    void Update()
    {
        if (newGameObjectsLists.Count > 0)
        {
            leafManager.OnLeafDrop();
        }
    }
}
