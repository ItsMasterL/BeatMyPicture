using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    PlayerManager parent;
    private void Awake()
    {
        parent = GetComponentInParent<PlayerManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject != parent.gameObject)
        {
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(parent.frametimer + 0.25f, parent.currentDamage); //TODO: move delay to template and make version specific
        }
    }
}
