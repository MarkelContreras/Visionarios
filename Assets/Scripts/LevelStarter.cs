using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStarter : MonoBehaviour {
    public int levelId;

    public void Play() {
        SceneManager.LoadScene(levelId);
    }

}
