using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    
    public int currentLevel;
    public int enemyCount;
    public GameObject goalPrefab;
    public GameObject scroll;

    private bool scrollFound = false;

    public static LevelManager instance;

    public static void Die() {
        SceneManager.LoadScene(0);
    }

    public static void Clear() {
        GameData.Get().levelsCompleted[instance.currentLevel - 1] = true;
        if (instance.scrollFound) GameData.Get().scrollsFound[instance.currentLevel - 1] = true;
        SceneManager.LoadScene(0);
    }

    public static void EnemyKilled() {
        if (--instance.enemyCount == 0) Instantiate(instance.goalPrefab, instance.transform.position, instance.transform.rotation);
    }

    public static void Scroll() {
        instance.scrollFound = true;
    }

    void Awakw() {
        instance = this;
        if (GameData.Get().scrollsFound[currentLevel - 1]) Destroy(scroll);
        if (enemyCount == 0) Instantiate(goalPrefab, transform.position, transform.rotation);
    }

}
