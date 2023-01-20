using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Character[] characters;

    public Character selectedCharacter;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (characters.Length > 0)
        {
            selectedCharacter = characters[0];
        }
    }

    public void SelectCharacter(Character character)
    {
        selectedCharacter = character;
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
