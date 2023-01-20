using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour
{

    /*public Text healthText;
    private float health;

    public Transform target;
    private PlayerStats playerStats;

    void Start()
    {
        findTarget();
    }

    void LateUpdate()
    {
        findTarget();
        health = playerStats.currentHealth;
        healthText.text = health.ToString();
    }

    private void findTarget(){
        if (target == null)
        {
            if (GameManager.instance.selectedCharacter != null){
                GameObject clone = GameObject.Find(GameManager.instance.selectedCharacter.name + "(Clone)");
                if (clone != null){
                    target = clone.transform;
                    playerStats = target.GetComponent<PlayerStats>();
                    health = playerStats.currentHealth;
                }
            }
            else{
                return;
            }
        }
    }*/
}
