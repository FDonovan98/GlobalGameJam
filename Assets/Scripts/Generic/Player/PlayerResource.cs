using UnityEngine;
using Photon.Pun;

public class PlayerResource
{
    public enum Resource
    {
        Health,
        Time
    }
    public GameObject player;
    public int maxHealth;
    protected int currentHealth;

    public float currentTime;

    public PlayerResource(GameObject setPlayer, int setMaxHealth = 1000, int startingTime = 300)
    {
        maxHealth = setMaxHealth;
        currentHealth = maxHealth;
        player = setPlayer;

        currentTime = (float)startingTime;
    }

    public void ChangePlayerResource(Resource type, float value)
    {
        if (type == Resource.Health)
        {
            if (currentHealth < -value)
            {
                Debug.LogWarning("You have died. Bad luck ol' chum");
            }
            else
            {
                currentHealth = Mathf.Min(currentHealth + (int)value, maxHealth);
            }

            Debug.Log(player.name + "health is " + currentHealth);
        }
        else if (type == Resource.Time)
        {
            if (currentTime < -value)
            {
                Debug.LogWarning("You have died. Bad luck ol' chum");
            }
            else
            {
                currentTime += value;
            }
        }

    }

}