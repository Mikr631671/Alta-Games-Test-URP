using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private PopupPanel winPanel;
    [SerializeField] private PopupPanel losePanel;

    private void OnEnable()
    {
        Character.onCharacterOnFinalPoint += winPanel.Show;
        CharacterShotProcessing.onCharacterCollapsed += losePanel.Show;
    }

    private void OnDisable()
    {
        Character.onCharacterOnFinalPoint -= winPanel.Show;
        CharacterShotProcessing.onCharacterCollapsed -= losePanel.Show;
    }
}
