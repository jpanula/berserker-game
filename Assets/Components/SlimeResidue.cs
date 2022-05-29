using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeResidue : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public float Age { get; private set; }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void Flipped(bool isFlipped)
    {
        _spriteRenderer.flipX = isFlipped;
    }
    
    public void ResetAge()
    {
        Age = 0;
    }
    
    private void Update()
    {
        Age += Time.deltaTime;
    }
}
