using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        StartCoroutine(RunTutorialAnimation());
    }

    public IEnumerator RunTutorialAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1f, .3f));

        Sequence scaleAnimation = DOTween.Sequence();
        scaleAnimation.Append(transform.GetChild(0).DOScale(1.05f, .45f));
        scaleAnimation.SetLoops(-1, LoopType.Yoyo);

        while (Input.GetMouseButtonDown(0) == false)
        {
            yield return null;
        }

        scaleAnimation.Kill();
        sequence.Kill();

        canvasGroup.DOFade(0f, .3f)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }
}
