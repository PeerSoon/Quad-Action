using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public int damage;
    public bool isMelee;
    public bool isRock;

    void OnCollisionEnter(Collision collision) 
    {
        if(!isRock && !isMelee && (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Statue"))
        {
            Destroy(gameObject, 2);
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if(!isMelee && (other.gameObject.tag == "Wall" || other.gameObject.tag == "Statue"))
        {
            Destroy(gameObject);
        }
    }
}
