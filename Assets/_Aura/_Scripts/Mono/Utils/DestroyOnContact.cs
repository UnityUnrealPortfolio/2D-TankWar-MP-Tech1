using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script desroys it's owner when
/// the owner's trigger makes contact
/// </summary>
public class DestroyOnContact : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
