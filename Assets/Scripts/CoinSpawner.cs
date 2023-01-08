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
        lastCoin = Instantiate(coinPrefab, transform.position, transform.rotation);
        return lastCoin.GetComponent<MoveMetalObject>();
    }

}
