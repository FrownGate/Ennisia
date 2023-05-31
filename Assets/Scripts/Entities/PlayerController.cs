using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Player Player { get; private set; }
    private Slider _hpBar;
    private void Start()
    {
        Player = new Player();
        InitHUD();
    }

    private void Update()
    {
        UpdateHUD();
    }

    private void InitHUD()
    {
        _hpBar = GetComponentInChildren<Slider>();
        _hpBar.maxValue = Player.MaxHp;
        _hpBar.value = Player.CurrentHp;
    }

    private void UpdateHUD()
    {
        _hpBar.value = Player.CurrentHp>=0 ?Player.CurrentHp : 0;
    }
    
    public void ResetStats()
    {
        Player = new Player();
    }
}