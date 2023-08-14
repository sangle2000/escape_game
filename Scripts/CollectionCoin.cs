using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionCoin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
