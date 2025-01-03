using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text scoreText;

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHpTxt(int hp)
    {
        hpText.text = $"HP: {hp}";
    }

    public void UpdateScoreTxt(int score)
    {
        scoreText.text = $"Score: {score}";
    }
    
}
