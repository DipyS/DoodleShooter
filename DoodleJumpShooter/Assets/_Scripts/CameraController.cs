using Cinemachine;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    public Player follow;
    public static Transform secondFollow;
    [SerializeField] float motionSpeed = 5;
    float motionProgress;
    void Start() {
        GameManager.onRestartGame.AddListener(onRestart);
    }

    void Update()
    {
        // Рассчитываем целевую позицию камеры
        Vector3 followPos = CalculateTargetPosition();

        // Обновляем прогресс движения
        if (secondFollow != null && !IsNearTarget(followPos))
        {
            motionProgress += motionSpeed * Time.deltaTime;
            motionProgress = Mathf.Clamp01(motionProgress);
            transform.position = Vector3.Lerp(transform.position, followPos, motionProgress);
        }
        else if (!IsNearTarget(followPos))
        {
            motionProgress -= motionSpeed * Time.deltaTime;
            motionProgress = Mathf.Clamp01(motionProgress);
            transform.position = Vector3.Lerp(followPos, transform.position, motionProgress);
        }
    }

    private Vector3 CalculateTargetPosition()
    {
        if (secondFollow != null)
        {
            return new Vector3(
                (follow.transform.position.x + secondFollow.position.x) / 2,
                (follow.transform.position.y + secondFollow.position.y) / 2,
                -10
            );
        }
        else
        {
            if (follow.transform.position.y > transform.position.y) return new Vector3(0, follow.transform.position.y, -10);
            else return new Vector3(0, transform.position.y, -10);
        }
    }

    private bool IsNearTarget(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) <= 0.05f;
    }

    void onRestart() {
        motionProgress = 0;
        transform.position = new Vector3(0, follow.transform.position.y, -10);
    }
}
