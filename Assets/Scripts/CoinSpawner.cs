using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour {
    
    public GameObject coinPrefab;

    private GameObject lastCoin = null;

    public MoveMetalObject SpawnCoin() {
        if (lastCoin != null) {
            Destroy(lastCoin);
        }
        lastCoin = Instantiate(coinPrefab, transform);
        return lastCoin.GetComponent<MoveMetalObject>();
    }

}
