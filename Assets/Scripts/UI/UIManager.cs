using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : Singleton<UIManager>
{
    private void Awake()
    {
        CreateInstance(this);
    }
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.Instance.OnChange += PlayerHealth_OnChange;
        GoldSystem.Instance.OnChange += GoldSystem_OnChange;
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        UpdateGold();
        UpdateHealth();
        UpdateTurn();
    }
    private void Update()
    {
        var countdown = TurnSystem.Instance.GetCountDown();
        if(countdown > 0)
        {
            timerText.text = countdown.ToString("0.0");
        }
        else
        {
            timerText.text = string.Empty;
        }
    }
    private void TurnSystem_OnTurnChange(object sender, int e)
    {
        UpdateTurn();
    }

    private void GoldSystem_OnChange(object sender, int e)
    {
        UpdateGold();
    }

    private void PlayerHealth_OnChange(object sender, float e)
    {
        UpdateHealth();
    }

    private void UpdateGold()
    {
        goldText.text = GoldSystem.Instance.GetGold().ToString();
    }
    private void UpdateHealth()
    {
        healthText.text = PlayerHealth.Instance.GetHealth().ToString();
    }
    
    private void UpdateTurn()
    {
        waveText.text = TurnSystem.Instance.GetTurn().ToString();
    }
}
