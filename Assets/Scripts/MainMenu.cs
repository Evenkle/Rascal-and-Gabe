using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject keybindingsImage;

    public void Awake() {
        Screen.SetResolution(600, 800, false);
    }

    public void play() {
        SceneManager.LoadScene("Game");
    }

    public void keybindings() {
        keybindingsImage.SetActive(true);
    }

    public void back() {
        keybindingsImage.SetActive(false);
    }

    public void quit() {
        Application.Quit();
    }

}
