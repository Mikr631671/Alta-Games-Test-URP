using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private CharacterShotProcessing characterShotProcessing;
    [SerializeField] private List<EnemyFieldCleaner> enemyFieldCleaners;

    void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        enemySpawner.Spawn();

        foreach (var enemyFieldCleaner in enemyFieldCleaners)
            enemyFieldCleaner.ClearEnemiesInZone();

        StartCoroutine(characterShotProcessing.StartShotProcessing());
    }
}
