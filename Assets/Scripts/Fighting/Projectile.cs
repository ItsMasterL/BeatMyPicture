using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    PlayerManager parent;

    public float lifetime = 5;
    public float damage = 0;
    public float deltax = 0;
    public float deltay = 0;

    private void Awake()
    {
        parent = GetComponentInParent<PlayerManager>();
    }



    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) Destroy(gameObject);

        GetComponent<Rigidbody2D>().velocity = new Vector2(deltax, deltay);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject != parent.gameObject)
        {
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(parent.frametimer + 0.25f, damage); //TODO: move delay to template and make version specific
            Destroy(gameObject);
        }
    }
}
