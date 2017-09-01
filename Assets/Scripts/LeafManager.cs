using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafManager : MonoBehaviour
{
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;
    LayerMask layerMask;
    public RaycastHit raycastHit;
    bool doDrop;//是否掉落
    [HideInInspector]
    public List<GameObject> newGameObjectsLists = new List<GameObject>();

    public bool DoDrop
    {
        get
        {
            return doDrop;
        }

        set
        {
            doDrop = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Leaf");
    }

    // Update is called once per frame
    void Update()
    {
        var direction = endPos.position - startPos.position;
        Ray ray = new Ray(startPos.position, direction);
        bool isCollider = Physics.Raycast(ray, out raycastHit, direction.magnitude, layerMask);
        if (isCollider)
        {
            DoDrop = false;
        }
        else
        {
            DoDrop = true;
        }

        if (DoDrop)
        {
            OnLeafDrop();
        }
    }

    public void OnLeafDrop()
    {
        if (newGameObjectsLists.Count > 0)
        {
            for (int i = 0; i < newGameObjectsLists.Count; i++)
            {
                if (newGameObjectsLists[i])
                {
                    SetGoValue(newGameObjectsLists[i]);
                }
            }
            newGameObjectsLists.Clear();
        }
    }

    void SetGoValue(GameObject go)
    {
        var rb = go.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
        go.GetComponent<CutObject>().enabled = false;
        go.GetComponent<ShatterTool>().enabled = false;
        go.GetComponent<TargetUvMapper>().enabled = false;
    }
}
