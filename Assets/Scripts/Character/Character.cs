using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Character : MonoBehaviour
{
    [Header("Configuration")]
    public float jumpHight = 5f;
    public float moveSpeed = 2f;

    [Header("Components")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ParticleSystem bulletExplosionEffect;
    private Rigidbody rb;

    public static Action onCharacterOnFinalPoint;

    private const int groundLayer = 1 << 8;

    private void OnEnable() =>
        CharacterShotProcessing.onRoadIsClear += OnRoadIsClear;

    private void OnDisable() =>
        CharacterShotProcessing.onRoadIsClear -= OnRoadIsClear;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        transform.localScale = Vector3.one * 2f; //Add size calculation logic
    }

    private void OnRoadIsClear()
    {
        rb.isKinematic = true;
        rb.constraints =
            RigidbodyConstraints.FreezeRotation |
            RigidbodyConstraints.FreezePositionX;

        StartCoroutine(JumpToTarget());
    }

    private IEnumerator JumpToTarget()
    {
        Transform target = FindObjectOfType<Door>().transform;

        transform.DOMoveY(transform.position.y + jumpHight, .3f).SetLoops(-1, LoopType.Yoyo);
        yield return transform.DOMoveZ(target.transform.position.z, 3.6f).WaitForCompletion();
        transform.DOKill();

        rb.isKinematic = false;
        onCharacterOnFinalPoint?.Invoke();
    }

    public void DestroyTheCharacter()
    {
        transform.DOScale(Vector3.zero, .3f)
           .SetEase(Ease.InBack)
           .OnComplete(() =>
           {
               audioSource.pitch = UnityEngine.Random.Range(0.85f, 1.15f);
               audioSource.Play();

               ParticleSystem newBulletExplosionEffect
                   = Instantiate(bulletExplosionEffect, transform.position, Quaternion.identity);
               Destroy(newBulletExplosionEffect.gameObject, 1f);

               Destroy(gameObject, .3f);
           });
    }
}
