using UnityEngine;
using Photon.Pun;

public class PlayerHealth
{
    public int maxHealth;
    protected int currentHealth;
    public GameObject player;

    public PlayerHealth(GameObject setPlayer, int setMaxHealth = 1000)
    {
        maxHealth = setMaxHealth;
        currentHealth = maxHealth;
        player = setPlayer;
    }

    public void ChangePlayerHealth(int damage)
    {
        if (currentHealth < damage)
        {
            Debug.LogWarning("You have died. Bad luck ol' chum");
        }
        else
        {
            currentHealth = Mathf.Min(currentHealth - damage, maxHealth);
        }

        Debug.Log(player.name + "health is " + currentHealth);
    }
}
