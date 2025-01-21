using System;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    [SerializeField] List<GeneratingBoss> objects = new List<GeneratingBoss>();
    
    void Start()
    {
        GameManager.onGenerate.AddListener(SpawnPlatforms);
    }
    void SpawnPlatforms() {
        //Генерация
        for (int i = 0; i < objects.Count; i++)
        {
            GenerateObject(objects[i]);
        }
     }

    public void GenerateObject(GeneratingBoss obj) {
        if (obj.needScores > GameManager.Instance.highScoresGame || obj.needScores + 86 <= GameManager.Instance.highScoresGame) return;

        if (obj.fixedBeCamera) Instantiate(obj.generatingObject, Camera.main.transform);
        else CameraController.secondFollow = Instantiate(obj.generatingObject, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 12, 0), Quaternion.identity).transform;
        
        foreach (var item in GameManager.objects)
        {
            if (item == null) continue;
            if (item.transform.position.y <= GameManager.Instance.player.transform.position.y) continue;
            if (item.GetComponent<Platform>() || item.CompareTag("Spring") || item.CompareTag("Rocket") || item.CompareTag("Cap") || item.CompareTag("Spike")) {
                item.GetComponent<Collider2D>().enabled = false;
                item.AddComponent<Rigidbody2D>().velocity = new Vector2(UnityEngine.Random.Range(-2f,3f),4);
                item.GetComponent<Rigidbody2D>().AddTorque(UnityEngine.Random.Range(-1000,1000));
            }
        }
    }
}

[Serializable]
public class GeneratingBoss {
    public GameObject generatingObject;
    public bool fixedBeCamera = true;
    public int needScores;
}
