using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : MonoBehaviour
{
    [SerializeField] GameObject knife;

    public void ShowKnife()
    {
        knife.SetActive(true);
    }

    public void HideKnife()
    {
        knife.SetActive(false);
    }
}
