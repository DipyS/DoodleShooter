using UnityEngine;
public class AdditionalCarousel : Carousel
{
    [SerializeField] float SinAdditionSpeed = 0.2f;
    [SerializeField] float CosAdditionSpeed = 0.2f;
    float sinAddTimer;
    float cosAddTimer;

    public new void Update()
    {
        base.Update();
        if (sinAddTimer <= 0) {
            sinAdd += 2;
            sinAddTimer = SinAdditionSpeed;
        } else {
            sinAddTimer -= Time.deltaTime;
        }
        if (cosAddTimer <= 0) {
            cosAdd += 2;
            cosAddTimer = CosAdditionSpeed;
        } else {
            cosAddTimer -= Time.deltaTime;
        }
    }
}
