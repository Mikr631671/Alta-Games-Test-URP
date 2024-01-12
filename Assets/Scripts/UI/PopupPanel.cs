using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PopupPanel : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float panelAnimationDuration = .3f;
    [SerializeField] private float continueButtonAnimationDuration = .45f;

    [Space(5)]
    [Header("Popup Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button continueButton;

    public void Show()
    {
        gameObject.SetActive(true);

        continueButton.transform.DOScale(1.05f, continueButtonAnimationDuration).SetLoops(-1, LoopType.Yoyo);
        canvasGroup.DOFade(1f, panelAnimationDuration);
    }

    public void Hide()
    {
        canvasGroup.DOFade(0f, panelAnimationDuration);
    }
}
