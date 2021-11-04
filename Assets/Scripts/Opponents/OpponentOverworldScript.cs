using System.Collections;
using System.Collections.Generic;
using Opponents;
using UnityEngine;
using UnityEngine.Serialization;

public class OpponentOverworldScript : MonoBehaviour
{
    [SerializeField, Header("Thresholds Scriptable Object '{}' for this opponent") ]private OpponentThresholds myThresholds;
    private SpriteRenderer _mainRenderer;
    private Color _mainColour;
    private SpriteRenderer _otherRenderer;
    private Color _otherColour;
    void Start()
    {
        _mainRenderer = GetComponent<SpriteRenderer>();
        _mainColour = _mainRenderer.color;
        _otherRenderer = GetComponentInChildren<SpriteRenderer>();
        _otherColour = _otherRenderer.color;
        int current = 0;
        StartCoroutine(ChangeColour(current));
    }

    IEnumerator ChangeColour(int current)
    {
        if (_otherColour.a == 1)
        {
            _otherColour.a = 0;
            _mainRenderer.sprite = _otherRenderer.sprite;
            current++;
            if (current == myThresholds.emotionSprites.Count - 1)
            {
                _otherRenderer.sprite = myThresholds.emotionSprites[0];
                current = 0;
            }
            else
            {
                _otherRenderer.sprite = myThresholds.emotionSprites[current + 1];
            }
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            _otherColour.a += 0.1f;
        }
    }
}
