using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Text txtLives;
    [SerializeField] private Text txtLevel;
    [SerializeField] private Text txtMoves;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject gameOverPanel;

    private PlayerMovement playerMovement;

    private void Start()
    {
        StartCoroutine(InitWhenPlayerExists());
    }

    private IEnumerator InitWhenPlayerExists()
    {
        while (playerMovement == null)
        {
            playerMovement = Object.FindFirstObjectByType<PlayerMovement>();
            yield return null;
        }
        SetupButtons();
    }

    private void SetupButtons()
    {
        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (var btn in buttons)
        {
            string nameLower = btn.gameObject.name.ToLower();

            if (nameLower.Contains("up"))
                btn.onClick.AddListener(OnUpPressed);
            else if (nameLower.Contains("down"))
                btn.onClick.AddListener(OnDownPressed);
            else if (nameLower.Contains("left"))
                btn.onClick.AddListener(OnLeftPressed);
            else if (nameLower.Contains("right"))
                btn.onClick.AddListener(OnRightPressed);
        }
    }

    #region UI Updates

    public void UpdateLevel(int level)
    {
        txtLevel.text = "Level: " + level;
    }

    public void UpdateLives(int lives)
    {
        txtLives.text = "Lives: " + lives;
    }

    public void UpdateMoves(int moves)
    {
        txtMoves.text = "Moves: " + moves;
    }

    public void ShowVictory()
    {
        victoryPanel.SetActive(true);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    #endregion UI Updates

    #region Input Handling

    private void OnUpPressed() => TryMove(0, 1);

    private void OnDownPressed() => TryMove(0, -1);

    private void OnLeftPressed() => TryMove(-1, 0);

    private void OnRightPressed() => TryMove(1, 0);

    private void TryMove(int dx, int dz)
    {
        if (playerMovement == null) return;

        playerMovement.TriggerMove(dx, dz);
    }

    #endregion Input Handling
}