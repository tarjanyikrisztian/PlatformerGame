using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenIngameMenu : MonoBehaviour
{

    public GameObject ingameMenu;
    public GameObject settingsMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ingameMenu.activeSelf == false)
            {
                if (settingsMenu.activeSelf == true)
                {
                    settingsMenu.SetActive(false);
                    ingameMenu.SetActive(true);
                }
                else
                {
                    Time.timeScale = 0;
                    ingameMenu.SetActive(true);
                }
            }
            else
            {
                if (settingsMenu.activeSelf == true)
                {
                    settingsMenu.SetActive(false);
                }
                else
                {
                    Time.timeScale = 1;
                    ingameMenu.SetActive(false);
                }
            }
        }
        
    }

    public void resume()
    {
        Time.timeScale = 1;
        ingameMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }
}
