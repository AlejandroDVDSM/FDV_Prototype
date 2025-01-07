using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Image hpImg;
    
    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Image scoreImg;
    
    [Header("End UI")]
    [SerializeField] private Image endPanel;
    [SerializeField] private TMP_Text endText;
    
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHpTxt(int hp)
    {
        hpText.text = $"x{hp}";
        hpImg.rectTransform.DOShakePosition(0.5f, 1f);
        hpImg.rectTransform.DOShakeRotation(0.5f, 1f);
        hpImg.rectTransform.DOShakeScale(0.5f, 1f);

    }

    public void UpdateScoreTxt(int score)
    {
        scoreText.text = $"x{score}";

        scoreImg.rectTransform.DORotate(new Vector3(0, 180, 0), 1f)
            .SetEase(Ease.InOutExpo)
            .SetLoops(2, LoopType.Yoyo);
    }

    public void DisplayEndPanel()
    {
        endPanel.DOFade(1.0f, 1.25f)
            .SetEase(Ease.OutSine);
        
        endText.DOFade(1.0f, 1.25f)
            .SetEase(Ease.OutSine);
    }
    
}
