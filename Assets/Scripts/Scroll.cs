using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {
    
    public void Found() {
        LevelManager.Scroll();
        Destroy(gameObject);
    }

}
