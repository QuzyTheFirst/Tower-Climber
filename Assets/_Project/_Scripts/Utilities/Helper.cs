using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public static void DeleteAllChildren(Transform tf)
    {
        foreach(Transform child in tf)
        {
            Destroy(child.gameObject);
        }
    }
}
