using UnityEngine;
using Photon.Pun;

using UnityEngine.UI;

public class PlayerResource
{
    public enum Resource
    {
        Health,
        Time,
    }
    public GameObject player;
    public int maxHealth;
    public int currentHealth;

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
                KillPlayer();
            }
            else
            {
                currentHealth = Mathf.Min(currentHealth + (int)value, maxHealth);
            }

            Text[] allUI = player.gameObject.GetComponentsInChildren<Text>();
            foreach (Text element in allUI)
            {
                if (element.gameObject.name == "TXT_Health")
                {
                    element.text = "Health: " + currentHealth;
                }
            }
        }
        else if (type == Resource.Time)
        {
            if (currentTime < -value)
            {
                KillPlayer();
            }
            else
            {
                currentTime += value;
            }

            Text[] allUI = player.gameObject.GetComponentsInChildren<Text>();
            foreach (Text element in allUI)
            {
                if (element.gameObject.name == "TXT_Time")
                {
                    element.text = "Time: " + (int)currentTime;
                }
            }
        }
    }

    private void KillPlayer()
    {
        Debug.LogWarning("You have died. Bad luck ol' chum");
        player.GetComponent<PlayerController>().shouldDie = true;
    }

}