using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosion;

    void Start()
    {
    }

    void Attack()
    {
        Instantiate(explosion,transform.position,Quaternion.identity);
        Destroy(gameObject);     
    }
}
