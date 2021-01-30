using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100f;

    public void reduceHealth(float amount)
    {
        if (health - amount < 0f)
        {
            health = 0f;
        }
        else
        {
            health -= amount;
        }
    }

    public void increaseHealth(float amount)
    {
        if (health + amount > 100f)
        {
            health = 100f;
        } else
        {
            health += amount;
        }
    }
}
