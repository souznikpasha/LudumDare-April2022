using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testmoon : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
    }
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Moonlight hits you.");
        }
    }
    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("The moonlight doesn't fall on you.");
        }
    }
}
