using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int numberEnemyInPlayZone = 5;
    [SerializeField] private int numberEnemyOutPlayZone = 5;

    [Space(5)]
    [SerializeField] private Vector3 spawnCenter = Vector3.zero;
    [SerializeField] private Vector3 spawnSquareSize = new Vector3(5f, 0f, 5f);
    [SerializeField] private float spawnHeight = 1f;
    [SerializeField] private float spawnRadius = 12f;

    [Header("Components")]
    [SerializeField] private Enemy activeEnemyPrefab;
    [SerializeField] private PassiveEnemy passiveEnemyPrefab;

    private readonly int maximumRewriteAttempts = 10;

    public void Spawn()
    {
        for (int i = 0; i < numberEnemyInPlayZone; i++)
        {
            Vector3 newPosition = GetSpawnPosition(spawnHeight, GetRandomPositionInSquare);

            Instantiate(activeEnemyPrefab, newPosition, Quaternion.identity, transform);
        }

        for (int i = 0; i < numberEnemyOutPlayZone; i++)
        {
            Vector3 newPosition = GetSpawnPosition(spawnHeight, GetRandomPositionInCircle);

            if (CheckPlayingField(newPosition.x))
                continue;

            PassiveEnemy newPassiveEnemyPrefab = Instantiate(passiveEnemyPrefab, newPosition, Quaternion.identity, transform);
            newPassiveEnemyPrefab.CustomizationView();
        }
    }

    private Vector3 GetSpawnPosition(float spawnHeight, Func<float, Vector3> spawnPositionFigure)
    {
        int overlappingCount = 0;
        Vector3 newPosition = Vector3.zero;

        while (overlappingCount < maximumRewriteAttempts)
        {
            newPosition = spawnPositionFigure(spawnHeight);

            if (IsOverlapping(newPosition) == false)
                break;

            overlappingCount++;
        }

        return newPosition;
    }

    private Vector3 GetRandomPositionInSquare(float height)
    {
        Vector3 newSquarePosition = Vector3.up * height;
        newSquarePosition.x = Random.Range(spawnCenter.x - spawnSquareSize.x / 2f, spawnCenter.x + spawnSquareSize.x / 2f);
        newSquarePosition.z = Random.Range(spawnCenter.z - spawnSquareSize.z / 2f, spawnCenter.z + spawnSquareSize.z / 2f);

        return newSquarePosition;
    }

    private Vector3 GetRandomPositionInCircle(float height)
    {
        Vector3 newCirclePosition = Vector3.up * height;
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float radius = Random.Range(0f, 1f) * spawnRadius;

        newCirclePosition.x = spawnCenter.x + radius * Mathf.Cos(angle);
        newCirclePosition.z = spawnCenter.z + radius * Mathf.Sin(angle);

        return newCirclePosition;
    }

    private bool IsOverlapping(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 1f, Enemy.enemyLayer);

        return colliders.Length > 0;
    }

    private bool CheckPlayingField(float xPosition)
        => Mathf.Abs(xPosition) < spawnSquareSize.x / 2f;
}