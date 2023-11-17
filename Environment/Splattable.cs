using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splattable : Ingredient
{
    [SerializeField] private float particleLifetime;
    [SerializeField] private int particleCount;
    private ParticleSystem pS;

    public override void splat()
    {
        if (pS)
        {
            pS.Emit(particleCount);
            //GetComponent<Collider>().enabled = false;
            //transform.GetChild(0).gameObject.SetActive(false);
            StartCoroutine(_destroy());
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator _destroy()
    {
        yield return new WaitForSeconds(particleLifetime);
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        if (transform.GetChild(1))
        {
            pS = transform.GetChild(1).GetComponent<ParticleSystem>();
        }
    }
}
