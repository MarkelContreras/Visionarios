using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBag : MonoBehaviour {

    public void HoverEnter() {
        GrabManager.TargetCoinSpawner();
    }

    public void HoverExit() {
        GrabManager.UntargetCoinSpawner();
    }
}
