using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterShotProcessing : MonoBehaviour
{
    [Header("Configuration")]
    [Range(.1f, 1f)]
    [SerializeField] private float startBulletSize = .3f;
    [Range(1f, 3f)]
    [SerializeField] private float bulletGrowthRate = 1f;
    [Range(1f, 10f)]
    [SerializeField] private float rateOfIncreaseInBulletVolumeSize = 1f;

    [Space(5)]
    [Range(0f, 1f)]
    [SerializeField] private float minimumCharacterSize = .2f;

    [Space(5)]
    [Header("Components")]
    [SerializeField] private Bullet bulletPrefab;
    private Character character;
    private Rigidbody rb;

    public static Action onRoadIsClear;
    public static Action onCharacterCollapsed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        character = FindObjectOfType<Character>();
    }

    public IEnumerator StartShotProcessing()
    {
        Enemy closestEnemy = RefreshEnemyTarget(null);

        while (true)
        {
            if (GetEnemyColliders().Length == 0)
            {
                onRoadIsClear?.Invoke();
                yield break;
            }

            closestEnemy = RefreshEnemyTarget(closestEnemy);

            while (Input.GetMouseButtonDown(0) == false)
            {
                yield return null;
            }

            float deltaSize = 0f;
            float startCharacterSize = character.transform.localScale.x;

            Bullet bullet = Instantiate(bulletPrefab, character.transform.position + Vector3.forward * 3f, Quaternion.identity);
            bullet.transform.localScale = Vector3.one * startBulletSize;

            rb.isKinematic = true;

            while (Input.GetMouseButtonUp(0) == false)
            {
                deltaSize += Time.deltaTime * bulletGrowthRate;
                bullet.transform.localScale = Vector3.one * (startBulletSize + (deltaSize * rateOfIncreaseInBulletVolumeSize));
                character.transform.localScale = Vector3.one * (startCharacterSize - deltaSize);
                character.transform.position = Vector3.Lerp(transform.position, Vector3.up * 5f, Time.deltaTime);

                closestEnemy = RefreshEnemyTarget(closestEnemy);

                if(CheckIfTheBulletIsPumped())
                {
                    bullet.DestroyTheBullet();
                    character.DestroyTheCharacter();

                    yield return new WaitForSeconds(.3f);
                    onCharacterCollapsed?.Invoke();
                    yield break;
                }

                yield return null;
            }

            rb.isKinematic = false;

            yield return bullet.ShootABullet(closestEnemy);
        }
    }

    private Enemy RefreshEnemyTarget(Enemy oldEnemy)
    {
        Enemy newEnemy = GetTheClosestEnemyInTheStrip();

        if (newEnemy == oldEnemy)
            return oldEnemy;

        if (newEnemy != null) newEnemy.SelectEnemy();
        if (oldEnemy != null) oldEnemy.UnSelectEnemy();

        return newEnemy;
    }

    private Enemy GetTheClosestEnemyInTheStrip()
    {
        Enemy nearestEnemy;
        Collider nearestCollider = null;
        float nearestDistance = float.MaxValue;
        Vector3 point = character.transform.position;

        Collider[] enemies = GetEnemyColliders();

        foreach (Collider enemyCollider in enemies)
        {
            float distance = Vector3.Distance(point, enemyCollider.ClosestPoint(point));

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestCollider = enemyCollider;
            }
        }

        if (nearestCollider == null)
            return null;

        nearestEnemy = nearestCollider.GetComponent<Enemy>();
        return nearestEnemy;
    }

    private Collider[] GetEnemyColliders()
    {
        Vector3 boxScale = new Vector3(transform.localScale.x * 1.7f, 10f, 100f);
        Collider[] enemieColliders
                = Physics.OverlapBox(transform.position, boxScale / 2f, Quaternion.Euler(Vector3.zero), Enemy.enemyLayer);

        return enemieColliders;
    }

    private bool CheckIfTheBulletIsPumped()
        => character.transform.localScale.x < minimumCharacterSize;
}
