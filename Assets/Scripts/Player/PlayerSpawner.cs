using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
    
    private void Start() {
        Instantiate(GameManager.instance.selectedCharacter.prefab, transform.position, Quaternion.identity);
    }

}