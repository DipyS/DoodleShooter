using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Material defauldMaterial;
    Material blink;
    void Start()
    {
        defauldMaterial = Resources.Load<Material>("Materials/DipyDefauld");
        blink = Resources.Load<Material>("Materials/Blink");
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
        
        StartCoroutine(Blink());
    }
    IEnumerator Blink() {
        if (spriteRenderer != null && defauldMaterial != null && blink != null) {
            spriteRenderer.material = blink;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.material = defauldMaterial;
        }
    }
}
