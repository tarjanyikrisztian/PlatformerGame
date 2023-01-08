using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour
{

    public Text healthText;
    private int health;

    private GameObject player;
    private PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerStats = player.GetComponent<PlayerStats>();
        health = playerStats.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = playerStats.currentHealth;
        healthText.text = "Health: " + health.ToString();
    }
}
