using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private CostumeSwapper _costumeSwapper;
    public CostumeSwapper CostumeSwapper { get { return _costumeSwapper; } }
}
