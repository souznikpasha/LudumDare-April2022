using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moonlight : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D other)
    {
        //БЛЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯ
        if (other.TryGetComponent(out Player player))
        {
            Debug.Log("Moonlight hits you.");
            player.SetMoonState(true);
        }
    }
    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Debug.Log("The moonlight doesn't fall on you.");
            player.SetMoonState(false);
        }
    }
}
