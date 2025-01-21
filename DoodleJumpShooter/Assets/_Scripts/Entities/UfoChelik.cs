using UnityEngine;

public class UfoChelik : Entity
{
    [SerializeField] Vector3[] startPoints;
    [SerializeField] float lerpSpeed = 0.1f;
    public bool isLerping {get; private set;}
    float lerpProgress;
    int startPos;
    [SerializeField] Animator childrenAnim;
    Animator anim;
    
    public new void Start()
    {
        base.Start();
        childrenAnim = GetComponentInChildren<Animator>();
        anim = GetComponent<Animator>();

        startPos = Random.Range(0, startPoints.Length);
        isLerping = true;
        if (Random.Range(0, 2) == 0) childrenAnim.SetTrigger("Robert");
        else childrenAnim.SetTrigger("CursedBoy");
        anim.SetInteger("RotateDirection", Random.Range(0,2));
    }

    void Update()
    {
        if (isLerping) 
        {
            lerpProgress += lerpSpeed * Time.deltaTime;
            lerpProgress = Mathf.Clamp01(lerpProgress);
            transform.position = new Vector2(
            Mathf.Lerp(startPoints[startPos].x + transform.parent.position.x, transform.parent.position.x, lerpProgress), 
            Mathf.Lerp(startPoints[startPos].y + transform.parent.position.y, transform.parent.position.y, lerpProgress)
            );

            if (lerpProgress >= 1) isLerping = false;
        }
    }
}
