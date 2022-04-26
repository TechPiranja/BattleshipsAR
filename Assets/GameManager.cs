using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject BoardPlacingPanel;
    public GameObject ShipPlacingPanel;
    private static string _currentPhase;
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("GameOver");
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void UpdateUI()
    {
        switch (_currentPhase)
        {
            case "shipPlacing":
                BoardPlacingPanel.SetActive(false);
                ShipPlacingPanel.SetActive(true);
                break;
        }
    }

    public void ChangePhase(string phase)
    {
        _currentPhase = phase;
        UpdateUI();
    }

    public string CurrentPhase()
    {
        return _currentPhase;
    }
}
