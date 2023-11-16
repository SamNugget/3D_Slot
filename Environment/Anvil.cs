using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anvil : Ingredient
{
    void OnCollisionEnter(Collision collision)
    {
        Ingredient i = collision.gameObject.GetComponent<Ingredient>();
        if (i != null)
        {
            i.splat();
        }
    }
}
