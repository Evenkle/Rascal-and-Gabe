using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

    public static int gridWidth = 10;
    public static int gridHeight = 21;

    public float dropSpeed = 0.5f;
    public float fallSpeed = 1f;
    public float respawnDealy = 0.2f;
    public float dropTimer = 3;
    public float fallTimer = 0;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    public GameObject[] tetriminos;

    public GameObject firstBlock;
    public GameObject secondBlock;
    public GameObject thirdBlock;
    private GameObject ghostBlock;

    public GameObject gabeScreen;
    public GameObject rascalScreen;
    public GameObject gabeExplotin;

    public bool isGameOver = false;
    public float gameOverScreenTime = 2;

    public float waterRaisingSpeed = 2;

    public enum Winner { Drowning, Squish, GabeAtTheTop, BlockAtTop };

    private Animator anim;

    void Start() {
        anim = GameObject.Find("Raccoon").GetComponent<Animator>();

        fillRandomTetrimino();
        spawnNextTetrimino();
    }

    public bool checkIsAbowGrid(Tetromino tetromino) {
        for (int x = 0; x < gridWidth; x++) {
            foreach (Transform mino in tetromino.transform) {
                Vector2 pos = round(mino.position);
                if(pos.y >= gridHeight) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool isFullRowAt(int y) {
        for (int x = 0; x < gridWidth; x++) {
            if(grid[x,y] == null) {
                return false;
            }
        }
        return true;
    }

    public void deleteMinoAt(int y) {
        for (int x = 0; x < gridWidth; x++) {
            Destroy(grid[x, y].gameObject);

            grid[x, y] = null;
        }
    }

    public void moveRowDown(int y) {
        for (int x = 0; x < gridWidth; x++) {
            if(grid[x, y] != null) {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void moveAllRowsDown(int y) {
        for (int i = y; i < gridHeight; i++) {
            moveRowDown(i);
        }
    }

    public void deleteRow() {
        for (int y = 0; y < gridHeight; y++) {
            if (isFullRowAt(y)) {
                deleteMinoAt(y);
                moveAllRowsDown(y+1);
                --y;
            }
        }
    }

    public void updateGrid(Tetromino tetromino) {
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                if(grid[x,y] != null) {
                    if (grid[x, y].parent == tetromino.transform) {
                        grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform mino in tetromino.transform) {
            Vector2 pos = round(mino.position);
            if(pos.y < gridHeight) {
                grid[(int)pos.x, (int)pos.y] = mino.transform;
            }
        }
    }

    public Transform getTransformAtGridPosition(Vector2 pos) {
        if(pos.y > gridHeight - 1) {
            return null;
        } else {
            return grid[(int)pos.x, (int)pos.y];
        }
    }

    private void fillRandomTetrimino() {
        firstBlock = tetriminos[Random.Range(0, tetriminos.Length)];
        secondBlock = tetriminos[Random.Range(0, tetriminos.Length)];
        thirdBlock = tetriminos[Random.Range(0, tetriminos.Length)];
    }

    public void spawnGhoisTetrimino() {
        if (GameObject.FindGameObjectWithTag("currentGhostTetrimino") != null) {
            Destroy(GameObject.FindGameObjectWithTag("currentGhostTetrimino"));
        }

        ghostBlock = (GameObject)Instantiate(firstBlock, new Vector2(5.0f, 20.0f), Quaternion.identity);

        Destroy(ghostBlock.GetComponent<Tetromino>());
        foreach (Transform mino in ghostBlock.transform) {
            Destroy(mino.GetComponent<BoxCollider2D>());
        }

        ghostBlock.AddComponent<GhostTetrimino>();
    }

    public void spawnNextTetrimino() {
        anim.SetTrigger("isAttacking");
        Instantiate(firstBlock, new Vector2(5.0f, 20.0f), Quaternion.identity);
        spawnGhoisTetrimino();
        firstBlock = secondBlock;
        secondBlock = thirdBlock;
        thirdBlock = tetriminos[Random.Range(0, tetriminos.Length)];

        fallTimer = Time.time;
    }

    public bool checkIsInsideGrid(Vector2 pos) {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    public Vector2 round(Vector2 pos) {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    public IEnumerator gameOver(Winner winner) {
        FindObjectOfType<CameraShake>().BigerShake(0.002f, 0.12f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Transform playerTransform = player.GetComponent<Transform>();
        SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer>();

        isGameOver = true;
        switch (winner) {
            case Winner.BlockAtTop:
                gabeScreen.SetActive(true);
                rb.isKinematic = true;
                rb.velocity = new Vector2(0, 0);
                playerTransform.position = new Vector3(4.5f, 22,0);
                break;
            case Winner.GabeAtTheTop:
                gabeScreen.SetActive(true);
                rb.isKinematic = true;
                rb.velocity = new Vector2(0, 0);
                playerTransform.position = new Vector3(4.5f, 22,0);
                break;
            case Winner.Drowning:
                rascalScreen.SetActive(true);
                rb.isKinematic = true;
                rb.velocity = new Vector2(0, 0);
                playerRenderer.enabled = false;
                Instantiate(gabeExplotin, playerTransform);
                break;
            case Winner.Squish:
                rascalScreen.SetActive(true);
                rb.isKinematic = true;
                rb.velocity = new Vector2(0, 0);
                playerRenderer.enabled = false;
                Instantiate(gabeExplotin, playerTransform);
                break;
        }

        yield return new WaitForSeconds(gameOverScreenTime);

        SceneManager.LoadScene("Main menu");
    }
}