using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionUI : MonoBehaviour
{
    public GameObject optionPrefab;
    public Transform prevCharacter;
    public Transform selectedCharacter;

    void Start()
    {
        foreach (Character character in GameManager.instance.characters)
        {
            GameObject option = Instantiate(optionPrefab, transform);
            Button button = option.GetComponent<Button>();

            button.onClick.AddListener(() => {
                GameManager.instance.SelectCharacter(character);
                if(selectedCharacter != null)
                {
                    prevCharacter = selectedCharacter;
                }

                selectedCharacter = option.transform;
            });

            Text text = option.GetComponentInChildren<Text>();
            text.text = character.name;

            Image image = option.GetComponentInChildren<Image>();
            image.sprite = character.portrait;


        }
    }
    void Update() {
        if (selectedCharacter != null) {
            selectedCharacter.localScale = Vector3.Lerp(selectedCharacter.localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 10);
        }

        if (prevCharacter != null) {
            prevCharacter.localScale = Vector3.Lerp(prevCharacter.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10);
        }
    }

}
