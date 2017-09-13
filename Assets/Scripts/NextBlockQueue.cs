using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextBlockQueue : MonoBehaviour {

    public Image first;
    public Image second;
    public Image third;

    public Sprite[] minis;

    private Game game;

	void Start () {
        game = FindObjectOfType<Game>();
	}
	
	void Update () {
        updateSprites();
	}

    private void updateSprites() {
        first.sprite = spriteSelector(game.firstBlock);
        second.sprite = spriteSelector(game.secondBlock);
        third.sprite = spriteSelector(game.thirdBlock);
    }

    private Sprite spriteSelector(GameObject block) {
        return block.GetComponent<Tetromino>().mini;
    }
}
