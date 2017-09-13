using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisingSea : MonoBehaviour {

    private float waterRaisingSpeed;
    public float timeIntervalBetweenSpeedups = 5;
    public float speedUpAmount = 0.0001f;
    private float lastTimeUpdate = 0;
    private float count = 1;

    private Game game;

    private void Start() {
        lastTimeUpdate = Time.time;
        game = FindObjectOfType<Game>();
        waterRaisingSpeed = 0.0001f * game.waterRaisingSpeed;
    }

    void Update () {
        transform.position += new Vector3(0, waterRaisingSpeed);

        if(Time.time - lastTimeUpdate >= timeIntervalBetweenSpeedups) {
            waterRaisingSpeed += count*speedUpAmount;
            lastTimeUpdate = Time.time;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            StartCoroutine(FindObjectOfType<Game>().gameOver(Game.Winner.Drowning));
        }
    }
}
