using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isMelee;
    public bool isRock;

    void OnCollisionEnter(Collision collision) 
    {
        if(!isRock && !isMelee && (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Statue" || collision.gameObject.tag == "Stairs"))
        {
            Destroy(gameObject, 3);
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if(!isMelee && (other.gameObject.tag == "Wall" || other.gameObject.tag == "Statue" || other.gameObject.tag == "Stairs"))
        {
            Destroy(gameObject);
        }
    }
}
