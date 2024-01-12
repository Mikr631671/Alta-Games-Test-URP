using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectProvider : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ParticleSystem doorParticleSystem;

    private void OnEnable() =>
        Character.onCharacterOnFinalPoint += OnCharacterOnFinalPoint;

    private void OnDisable() =>
        Character.onCharacterOnFinalPoint -= OnCharacterOnFinalPoint;

    public void OnCharacterOnFinalPoint()
    {
        doorParticleSystem.Play();
        //Add Audio
    }
}
