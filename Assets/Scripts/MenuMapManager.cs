using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMapManager : MonoBehaviour
{
    public GameObject[] levelTicks;
    public GameObject[] scrollTicks;
    public GameObject[] levelLoreImages;
    public GameObject[] levelLoreTexts;
    public GameObject[] scrollLoreImages;
    public GameObject[] scrollLoreTexts;
    public GameObject lastLevelPlayButton;
    public PlayerMovement character;
    public Transform menuPosition;

    // Start is called before the first frame update
    void Awake() {
        bool destroyLastLevelPlayButton = false;
        for (int i = 0; i < levelTicks.Length; i++) {
            if (!GameData.Get().levelsCompleted[i]) {
                Destroy(levelTicks[i]);
                if (i == 8) continue; //level 9 doesn't have lore scrolls
                Destroy(levelLoreImages[i]);
                Destroy(levelLoreTexts[i]);
                destroyLastLevelPlayButton = true;
            } 
        }
        for (int i = 0; i < scrollTicks.Length; i++) {
            if (!GameData.Get().scrollsFound[i]) {
                Destroy(scrollTicks[i]);
                Destroy(scrollLoreImages[i]);
                Destroy(scrollLoreTexts[i]);
            }
        }
        if (destroyLastLevelPlayButton) Destroy(lastLevelPlayButton);
        GameData.Save();
    }

    void Start() {
        if (GameData.Get().tutorialDone) {
            character.Warp(menuPosition);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject == character.gameObject) {
            GameData.Get().tutorialDone = true;
            GameData.Save();
        }
    }
}
