using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using DG.Tweening;
using Lofelt.NiceVibrations;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ParticleSystem hitParticleSystem;
    [SerializeField] private SphereCollider sphereCollider;

    [Space(5)]
    private Material material;
    [SerializeField] private Material virusInfectedMaterial;

    public const int enemyLayer = 1 << 7;

    private void Awake()
    {
        material = meshRenderer.materials[0];
    }

    public void InfectTheEnemy()
    {
        sphereCollider.enabled = false;

        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(Random.Range(.1f, .3f));
        sequence.Append(material.DOColor(virusInfectedMaterial.GetColor("_BaseColor"), "_BaseColor", .3f));
        sequence.Join(material.DOColor(virusInfectedMaterial.GetColor("_HColor"), "_HColor", .3f));
        sequence.Join(material.DOColor(virusInfectedMaterial.GetColor("_SColor"), "_SColor", .3f));
        sequence.Join(material.DOColor(virusInfectedMaterial.GetColor("_RimColor"), "_RimColor", .3f));
        sequence.Join(material.DOColor(virusInfectedMaterial.GetColor("_SpecularColor"), "_SpecularColor", .3f));

        sequence.SetLoops(2, LoopType.Yoyo);
        sequence.OnComplete(OnEnemyHit);

        sequence.Play();
    }

    public void OnEnemyHit()
    {
        meshRenderer.materials[0].DOFloat(5f, "_OutlineWidth", .3f);

        transform.DOScale(Vector3.zero, .3f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                audioSource.pitch = Random.Range(0.85f, 1.15f);
                audioSource.Play();

                HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

                ParticleSystem newHitParticleSystem
                    = Instantiate(hitParticleSystem, transform.position, Quaternion.identity);
                Destroy(newHitParticleSystem.gameObject, 1f);

                Destroy(gameObject, .3f);
            });
    }

    public void SelectEnemy()
    {
        meshRenderer.materials[0].SetFloat("_OutlineWidth", 5f);
    }

    public void UnSelectEnemy()
    {
        meshRenderer.materials[0].SetFloat("_OutlineWidth", 0f);
    }
}
