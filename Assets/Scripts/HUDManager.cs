using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class HUDManager : MonoBehaviour
{
    private Slider p1HPBar;
    private Slider p2HPBar;
    private TMP_Text middleText;
    private TMP_Text timer;
    private PlayerInput p1InputController;
    private PlayerInput p2InputController;
    private Boolean gameRunning = true;

    [SerializeField] private float gameTime = 90;

    private void Awake() {
        p1HPBar = transform.Find("P1HPBar").GetComponent<Slider>();
        p2HPBar = transform.Find("P2HPBar").GetComponent<Slider>();
        middleText = transform.Find("MiddleText").GetComponent<TMP_Text>();
        timer = transform.Find("Timer").GetComponent<TMP_Text>();
        p1InputController = GameObject.Find("Player1").GetComponent<PlayerInput>();
        p2InputController = GameObject.Find("Player2").GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (!gameRunning)
            return;

        gameTime -= Time.deltaTime;

        timer.text = ((int)gameTime).ToString();

        if (gameTime <= 0) {
            ShowMiddleText();
        }
    }

    private void OnEnable()
    {
        Damageable.OnDamageTaken += DecreaseHPBar;
    }

    private void OnDisable()
    {
        Damageable.OnDamageTaken -= DecreaseHPBar;
    }

    private void DecreaseHPBar(float hp, String tag) {
        if (tag == "Player1") 
            p1HPBar.value = hp;
        else
            p2HPBar.value = hp;

        if (hp <= 0) 
            ShowMiddleText();
    }

    private void ShowMiddleText() {
        middleText.enabled = true;
        if (p1HPBar.value < p2HPBar.value) 
            middleText.text = "P2 Wins!";
        else if (p1HPBar.value > p2HPBar.value)
            middleText.text = "P1 Wins!";
        else
            middleText.text = "Draw!";

        StartCoroutine(endGame());
    }

    IEnumerator endGame() {
        gameRunning = false;
        p1InputController.SwitchCurrentActionMap("Disabled");
        p2InputController.SwitchCurrentActionMap("Disabled");
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
