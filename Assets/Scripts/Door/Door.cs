using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [Header("Configuration")]
    [Range(1f, 30f)]
    [SerializeField] private float doorOpeningDistance = 5f;

    [Space(5)]
    [Header("Components")]
    [SerializeField] private Transform doorTransform;

    private void OnEnable()
    {
        Character.onCharacterOnFinalPoint += CloseTheDoor;
        CharacterShotProcessing.onRoadIsClear += OpenTheDoorWrapper;
    }

    private void OnDisable()
    {
        Character.onCharacterOnFinalPoint += CloseTheDoor;
        CharacterShotProcessing.onRoadIsClear -= OpenTheDoorWrapper;
    }

    void OpenTheDoorWrapper()
    {
        StartCoroutine(OpenTheDoor());
    }

    public IEnumerator OpenTheDoor()
    {
        Character character = FindObjectOfType<Character>();

        while (Vector3.Distance(transform.position, character.transform.position) > doorOpeningDistance)
            yield return null;

        doorTransform.DOLocalRotate(Vector3.up * 100f, .6f).SetEase(Ease.OutQuad);
    }

    public void CloseTheDoor()
        => doorTransform.DOLocalRotate(Vector3.zero, .6f).SetEase(Ease.OutQuad);
}
