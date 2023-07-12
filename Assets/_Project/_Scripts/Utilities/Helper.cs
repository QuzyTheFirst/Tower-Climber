using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;

public class Helper : MonoBehaviour
{
    public static void DeleteAllChildren(Transform tf)
    {
        foreach(Transform child in tf)
        {
            Destroy(child.gameObject);
        }
    }

    public static IEnumerable<T> FindInterfacesOfType<T>(bool includeInactive = false) 
    { 
        return SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(go => go.GetComponentsInChildren<T>(includeInactive)); 
    }
}
