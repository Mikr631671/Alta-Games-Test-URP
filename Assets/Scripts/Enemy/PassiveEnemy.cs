using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEnemy : MonoBehaviour
{
    public void CustomizationView()
    {
        if (Random.Range(0, 100) < 33)
            transform.localScale += Vector3.up * Random.Range(.1f, .2f);
    }
}
