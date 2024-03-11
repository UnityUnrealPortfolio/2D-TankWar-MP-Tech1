using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script desroys it's owner after a 
/// set time
/// </summary>
public class Lifetime : MonoBehaviour
{
    [Header("Time to self destruct")]
    [SerializeField] private float lifeTime;

    private void Awake()
    {
        Destroy(gameObject,lifeTime);
    }
}
