using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RascalWinnTriggerZone : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" ) {
            StartCoroutine(FindObjectOfType<Game>().gameOver(Game.Winner.Drowning));
        }
    }
}
