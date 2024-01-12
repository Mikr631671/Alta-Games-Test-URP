using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lofelt.NiceVibrations;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Configuration")]
    [Range(1f, 5f)]
    [SerializeField] private float infectionRadiusMultiplier = 1f;
    [SerializeField] private AnimationCurve chanceSpreadPoison;

    [Space(5)]
    [Header("Components")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ParticleSystem bulletExplosionEffect;

    public void Charging(float newChargingFactor)
        => transform.localScale = Vector3.one * newChargingFactor;

    public IEnumerator ShootABullet(Enemy target)
    {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveZ(target.transform.position.z, .3f).SetEase(Ease.Linear));
        sequence.Join(transform.DOMoveY(target.transform.position.y + .3f, .3f).SetEase(Ease.InQuart));
        sequence.Join(transform.DOMoveX(target.transform.position.x, .3f).SetEase(Ease.InQuart));
        sequence.Play();

        yield return sequence.WaitForCompletion();
        yield return UltimateStrike();

        if (target != null) target.OnEnemyHit();
        Destroy(gameObject);
    }

    public IEnumerator UltimateStrike()
    {
        float ultimateRadius = (transform.localScale.x / 2f) * infectionRadiusMultiplier;

        Collider[] enemyColliders
            = Physics.OverlapSphere(transform.position, ultimateRadius, Enemy.enemyLayer);

        foreach (var enemyCollider in enemyColliders)
        {
            float currentDistance = Vector3.Distance(transform.position, enemyCollider.transform.position);
            float normalizedValue = Mathf.Clamp01(currentDistance / ultimateRadius);

            if (Random.Range(0f, 1f) <= chanceSpreadPoison.Evaluate(normalizedValue))
            {
                //The enemy is infected

                Enemy infectedEnemy = enemyCollider.GetComponent<Enemy>();

                if (infectedEnemy == null)
                {
                    //Debug.LogError("Enemy Search Error");
                    continue;
                }

                infectedEnemy.InfectTheEnemy();

            }

            //The enemy is not infected
        }

        yield return null;
    }

    public void DestroyTheBullet()
    {
        transform.DOScale(Vector3.zero, .3f)
           .SetEase(Ease.InBack)
           .OnComplete(() =>
           {
               audioSource.pitch = Random.Range(0.85f, 1.15f);
               audioSource.Play();

               ParticleSystem newBulletExplosionEffect
                   = Instantiate(bulletExplosionEffect, transform.position, Quaternion.identity);
               Destroy(newBulletExplosionEffect.gameObject, 1f);

               Destroy(gameObject, .3f);
           });
    }
}