using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player follow;
    public static Transform secondFollow;
    bool previousSecondFollow;
    public CinemachineVirtualCamera mainCamera;
    [SerializeField] float size = 1.5f;
    [SerializeField] float motionSpeed = 5;
    float motionProgress;
    void Start()
    {
        mainCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        Vector3 followPos = transform.position;
        if (secondFollow != null) {
            if (motionProgress < 1) motionProgress += motionSpeed * Time.deltaTime;

            followPos = new Vector3((follow.transform.position.x + secondFollow.position.x) / 2, (follow.transform.position.y + secondFollow.position.y) / 2, -10);
            mainCamera.m_Lens.OrthographicSize = Vector2.Distance(follow.transform.position, secondFollow.transform.position) * size;

            transform.position = Vector3.Lerp(new Vector3(0, follow.transform.position.y, -10), followPos, motionProgress);
            
            previousSecondFollow = true;
        } 
        else if (follow.transform.position.y > transform.position.y) {
            if (motionProgress > 0) motionProgress -= motionSpeed * Time.deltaTime;

            mainCamera.m_Lens.OrthographicSize = 7.44f;
            transform.position = Vector3.Lerp(new Vector3(0, follow.transform.position.y, -10), followPos, motionProgress);

            previousSecondFollow = false;
        }
    }
}
