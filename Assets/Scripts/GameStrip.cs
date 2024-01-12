using UnityEngine;

public class GameStrip : MonoBehaviour
{
    [Header("Components")]
    private Character character;

    [Space(5)]
    private float characterSizeCoefficient;


    private void Awake()
    {
        characterSizeCoefficient = Mathf.PI / 2f;
        character = FindObjectOfType<Character>();
    }

    private void LateUpdate()
    {
        if(character != null)
            transform.localScale =
                new Vector3(character.transform.localScale.x * characterSizeCoefficient, transform.localScale.y, transform.localScale.z);
    }
}
