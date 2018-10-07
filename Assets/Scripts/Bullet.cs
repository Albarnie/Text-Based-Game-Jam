using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 lastPos = Vector3.zero;
    public Gun gun;
    public LayerMask mask;
    public GameObject hitPrefab, bloodPrefab;
    int continues;
    public int damage;

    private void Awake()
    {
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Linecast(lastPos, transform.position, mask);
        if(hit.collider != null)
        {

            Vector3 inPoint = hit.point;
            //Vector3 outPoint = Physics2D.Linecast(transform.position, lastPos, mask).point;
            if (continues < 2)
            {
                Instantiate(hitPrefab, inPoint, Quaternion.LookRotation(hit.normal));
                //Instantiate(hitPrefab, outPoint, Quaternion.LookRotation(-hit.normal));
            }
            Debug.DrawLine(inPoint, transform.position, Color.green, 2);
            switch(hit.collider.tag)
            {
                case "Enemy":
                    Instantiate(bloodPrefab, inPoint, Quaternion.LookRotation(-hit.normal));
                    hit.collider.GetComponent<Enemy>().Damage(gun.bulletDamage /(continues +1));
                    break;
                case "Player":
                    Instantiate(bloodPrefab, inPoint, Quaternion.LookRotation(-hit.normal));
                    hit.collider.GetComponent<Player>().Damage(gun.bulletDamage /(continues +1));
                    break;
            }
            GetComponent<Rigidbody2D>().velocity *= 0.9f;

            continues++;
        }
        lastPos = transform.position;
        if (GetComponent<Rigidbody2D>().velocity.magnitude < 2 && continues > 0 || continues > 3 || damage <1 && continues > 0)
            Destroy(gameObject);
    }
}
