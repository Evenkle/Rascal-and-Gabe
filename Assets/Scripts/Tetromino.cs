using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

    public bool allowRotation = true;
    public bool limitRotation = false;

    private bool isDroped = false;
    private bool canRotate = true;

    private float fall = 0;

    public Sprite mini;

    Game game;

    void Start () {
        game = FindObjectOfType<Game>();
        tag = "currentActiveTetrimino";
	}
	
	void Update () {
        if (!game.isGameOver) {
            checkUserInput();
        }
	}

    void checkUserInput() {
        if (Input.GetKeyDown(KeyCode.D) && !isDroped) {

            transform.position += new Vector3(1, 0, 0);
            if (checkIsValidPosition()) {
            } else {
                transform.position += new Vector3(-1, 0, 0);
            }

        } else if (Input.GetKeyDown(KeyCode.A) && !isDroped) {
            transform.position += new Vector3(-1, 0, 0);
            if (checkIsValidPosition()) {
            } else {
                transform.position += new Vector3(1, 0, 0);
            }


        } else if (Input.GetKeyDown(KeyCode.W) && !isDroped) {

            if (allowRotation) {
                if (limitRotation) {
                    if (transform.rotation.eulerAngles.z >= 90) {
                        transform.Rotate(0, 0, -90);
                        if (checkIsValidPosition()) {
                            foreach (Transform mino in transform) {
                                mino.Rotate(0, 0, 90);
                            }
                        } else {
                            transform.Rotate(0, 0, 90);
                        }
                    } else {
                        transform.Rotate(0, 0, 90);
                        if (checkIsValidPosition()) {

                            foreach (Transform mino in transform) {
                                mino.Rotate(0, 0, -90);
                            }
                        } else {
                            transform.Rotate(0, 0, -90);
                        }

                    }
                } else {
                    transform.Rotate(0, 0, 90);
                    if (checkIsValidPosition()) {

                        foreach (Transform mino in transform) {
                            mino.Rotate(0, 0, -90);
                        }
                    } else {
                        transform.Rotate(0, 0, -90);
                    }
                }
            }
        } else if (Input.GetKeyDown(KeyCode.Space) && !isDroped) {
            isDroped = true;
            StartCoroutine(theDrop());
            StartCoroutine(spawnNext());
        } else if (Time.time - fall >= game.fallSpeed && !isDroped) {
            transform.position += new Vector3(0, -1, 0);

            if (checkIsValidPosition()) {
                game.updateGrid(this);
            } else {
                transform.position += new Vector3(0, 1, 0);

                game.deleteRow();

                StartCoroutine(spawnNext());

                if (game.checkIsAbowGrid(this) && tag != "currentGhostTetrimino") {
                    StartCoroutine(game.gameOver(Game.Winner.BlockAtTop));
                    game.isGameOver = true;
                }

                enabled = false;
            }
            fall = Time.time;
        }
    }

    private IEnumerator spawnNext() {
        if (isDroped) {
            yield return new WaitForSeconds(game.respawnDealy + game.fallSpeed);
        }
        tag = "Untagged";
        if (!game.isGameOver) {
            game.spawnNextTetrimino();
        }
    }

    private IEnumerator theDrop() {
        while (true) {
            transform.position += new Vector3(0, -1, 0);

            if (checkIsValidPosition()) {
                game.updateGrid(this);
            } else {
                transform.position += new Vector3(0, 1, 0);
               
                game.deleteRow();

                if (game.checkIsAbowGrid(this) && tag != "currentGhostTetrimino") {
                    StartCoroutine(game.gameOver(Game.Winner.BlockAtTop));
                    game.isGameOver = true;
                }

                enabled = false;
                break;
            }

            yield return new WaitForSeconds(game.dropSpeed);
        }
    }

    bool checkIsValidPosition() {
        Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        foreach (Transform mino in transform) {
            Vector2 pos = game.round(mino.position);
            


            if (game.checkIsInsideGrid(pos) == false) {
                return false;
            }

            if(game.getTransformAtGridPosition(pos) != null && game.getTransformAtGridPosition(pos).parent != transform) {
                return false;
            }

            if (Mathf.Abs(playerPos.x - pos.x) < 0.7f && Mathf.Abs(playerPos.y - pos.y) <= 0.1f) {
                StartCoroutine(game.gameOver(Game.Winner.Squish));
            }
        }
        return true;
    }
}
