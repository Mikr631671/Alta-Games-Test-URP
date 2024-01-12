using System.Collections;
using System.Collections.Generic;
using Lofelt.NiceVibrations;
using UnityEngine;

public class Player : MonoBehaviour
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

    [Header("Components")]
    [SerializeField] private TouchControl touchControl;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform shootPosition;
    private Character character;
    private Rigidbody rb;

    private Coroutine chargingBullet;
    private Bullet bullet;
    private Enemy closestEnemy;

    private void Awake()
    {
        touchControl.TouchDown += OnTouchDown;
        touchControl.TouchUp += OnTouchUp;

        rb = GetComponent<Rigidbody>();
        character = FindObjectOfType<Character>();
    }

    private void OnTouchUp()
    {
        StopCoroutine(chargingBullet);

        rb.isKinematic = false;

        StartCoroutine(bullet.ShootABullet(closestEnemy));
    }

    private void OnTouchDown()
    {
        bullet = Instantiate(bulletPrefab, shootPosition.position, Quaternion.identity);
        bullet.transform.localScale = Vector3.one * startBulletSize;

        rb.isKinematic = true;

        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

        chargingBullet = StartCoroutine(ChargingBullet(bullet));
    }

    private IEnumerator ChargingBullet(Bullet bullet)
    {
        float deltaSize = 0f;
        float startCharacterSize = character.transform.localScale.x;

        while (true)
        {
            deltaSize += Time.deltaTime * bulletGrowthRate;

            float newBulletChargingFactor = startBulletSize + (deltaSize * rateOfIncreaseInBulletVolumeSize);

            bullet.Charging(newBulletChargingFactor);

            character.transform.localScale = Vector3.one * (startCharacterSize - deltaSize);
            character.transform.position = Vector3.Lerp(transform.position, Vector3.up * 5f, Time.deltaTime);

            closestEnemy = GetEnemyTarget(closestEnemy);

            if (CheckIfThePlayerIsPumped())
            {
                bullet.DestroyTheBullet();
                character.DestroyTheCharacter();

                yield return new WaitForSeconds(.6f);
                //onCharacterCollapsed?.Invoke();
                yield break;
            }

            yield return null;
        }
    }

    private Enemy GetEnemyTarget(Enemy oldEnemy)
    {
        Enemy newEnemy = oldEnemy;


        return newEnemy;
    }

    private bool CheckIfThePlayerIsPumped()
        => character.transform.localScale.x < minimumCharacterSize;
}
