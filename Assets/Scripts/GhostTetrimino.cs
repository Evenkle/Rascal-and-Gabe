using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTetrimino : MonoBehaviour {

	void Start () {
        tag = "currentGhostTetrimino";

        foreach(Transform mino in transform) {
            mino.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .2f);
        }
	}
	
	void Update () {
        FollowActiveTetromino();
        moveDown();
	}

    void FollowActiveTetromino() {

        Transform currentActiveTetriminoTransform = GameObject.FindGameObjectWithTag("currentActiveTetrimino").transform;
        transform.position = currentActiveTetriminoTransform.position;
        transform.rotation = currentActiveTetriminoTransform.rotation;

    }

    void moveDown() {
        while (checkisValidPosition()) {
            transform.position += new Vector3(0, -1, 0);
        }
        if (!checkisValidPosition()) {
            transform.position += new Vector3(0, 1, 0);
        }

    }

    bool checkisValidPosition() {

        foreach (Transform mino in transform) {

            Vector2 pos = FindObjectOfType<Game>().round(mino.position);

            if (FindObjectOfType<Game>().checkIsInsideGrid(pos) == false) {
                return false;
            }

            if (FindObjectOfType<Game>().getTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().getTransformAtGridPosition(pos).parent.tag == "currentActiveTetrimino")
                return true;

            if (FindObjectOfType<Game>().getTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().getTransformAtGridPosition(pos).parent != transform)
                return false;
        }

        return true;
    }

    /*
    if (FindObjectOfType<Game>().getTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().getTransformAtGridPosition(pos).parent.tag != "currentActiveTetrimino") {
        return true;
    }

    if (FindObjectOfType<Game>().getTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().getTransformAtGridPosition(pos).parent != transform) {
        return false;
    }*/
}
