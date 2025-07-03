using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs & References")]
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Vector3 spawnLocation;

    [Header("Environment")]
    [SerializeField] private string groundName = "Ground";

    [SerializeField] private string winSpotName = "WinSpot";

    [Header("UI")]
    [SerializeField] private Canvas levelCanvas;

    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Victory Panel Buttons")]
    [SerializeField] private Button v_HomeBtn;

    [SerializeField] private Button v_ResetBtn;
    [SerializeField] private Button v_NextBtn;

    [Header("GameOver Panel Buttons")]
    [SerializeField] private Button g_HomeBtn;

    [SerializeField] private Button g_ResetBtn;

    [Header("Game Data")]
    [SerializeField] private int lives = 3;

    [SerializeField] private int level;
    public static int moves;

    private GameObject currentPlayer;
    private GameObject ground;
    private GameObject winSpot;
    private bool checkingWin = true;
    private LevelUI ui;

    private VictoryEffect victoryEffect;

    private void Awake()
    {
        ground = GameObject.Find(groundName);
        winSpot = GameObject.Find(winSpotName);

        foreach (var btn in victoryPanel.GetComponentsInChildren<Button>())
        {
            switch (btn.gameObject.name)
            {
                case "HomeButton":
                    btn.onClick.AddListener(OnHomeButton);
                    break;

                case "ResetButton":
                    btn.onClick.AddListener(OnResetButton);
                    break;

                case "NextButton":
                    btn.onClick.AddListener(OnNextButton);
                    break;
            }
        }

        foreach (var btn in gameOverPanel.GetComponentsInChildren<Button>())
        {
            switch (btn.gameObject.name)
            {
                case "HomeButton":
                    btn.onClick.AddListener(OnHomeButton);
                    break;

                case "ResetButton":
                    btn.onClick.AddListener(OnResetButton);
                    break;

                case "exitButton":
                    btn.onClick.AddListener(OnExitButton);
                    break;
            }
        }
    }

    private void Start()
    {
        victoryEffect = GetComponent<VictoryEffect>();

        ui = levelCanvas.GetComponent<LevelUI>();
        ui.UpdateLives(lives);
        ui.UpdateLevel(level);
        ui.UpdateMoves(moves);

        var rend = winSpot.GetComponent<MeshRenderer>();
        if (rend != null) rend.enabled = false;

        victoryPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        currentPlayer = Instantiate(playerPrefab, spawnLocation, Quaternion.identity);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (currentPlayer != null && checkingWin)
        {
            bool atWin =
                Mathf.Abs(currentPlayer.transform.position.x - winSpot.transform.position.x) < 0.1f &&
                Mathf.Abs(currentPlayer.transform.position.z - winSpot.transform.position.z) < 0.1f &&
                !currentPlayer.GetComponent<PlayerMovement>().isRotating;

            if (atWin)
                OnWin();
            else if (currentPlayer.transform.position.y < -100f)
                PlayerDead();
            else if (currentPlayer.transform.position.y < -4f)
                currentPlayer.GetComponent<BoxCollider>().isTrigger = true;
        }

        if (lives > 0)
            ui.UpdateMoves(moves);

        if (Input.GetKey(KeyCode.R))
            OnResetButton();

        if (Input.GetKey(KeyCode.Escape))
            OnHomeButton();
    }

    private void OnWin()
    {
        if (winSpot != null)
            VictoryEffect.SpawnAt(winSpot.transform.position);
        checkingWin = false;
        currentPlayer.GetComponent<PlayerMovement>().enabled = false;
        currentPlayer.GetComponent<Rigidbody>().freezeRotation = true;
        currentPlayer.GetComponent<BoxCollider>().isTrigger = true;

        StartCoroutine(WinSequence());
    }

    private IEnumerator WinSequence()
    {
        yield return new WaitForSecondsRealtime(3f);

        if (ground != null) ground.SetActive(false);
        if (winSpot != null) winSpot.SetActive(false);
        if (currentPlayer != null) currentPlayer.SetActive(false);

        Time.timeScale = 0f;
        victoryPanel.SetActive(true);
    }

    public void PlayerDead()
    {
        lives--;
        ui.UpdateLives(lives);
        Destroy(currentPlayer);

        if (lives > 0)
            currentPlayer = Instantiate(playerPrefab, spawnLocation, Quaternion.identity);
        else
            StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        if (ground != null) ground.SetActive(false);
        if (winSpot != null) winSpot.SetActive(false);
        if (currentPlayer != null) currentPlayer.SetActive(false);

        moves = 0;
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        yield break;
    }

    private void OnHomeButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnResetButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnNextButton()
    {
        Time.timeScale = 1f;
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            SceneManager.LoadScene(0);
    }

    private void OnExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}