using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] List<GeneratingObject> objects = new List<GeneratingObject>();
    void Start()
    {
        GameManager.onGenerate.AddListener(SpawnPlatforms);
        GameManager.onRestartGame.AddListener(RestartGame);
    }
    void SpawnPlatforms() {
        //Генерация
        for (int i = 0; i < objects.Count; i++)
        {
            GenerateObject(objects[i].generatingObject,objects[i].count,objects[i].needScores,objects[i].maxNeedScores,objects[i].generateChance);
        }
     }

    public void GenerateObject(GameObject generatingObject,int count,int needScores,int maxNeedScores ,int generateChance = 100) {
        if (needScores > GameManager.Instance.highScoresGame || maxNeedScores <= GameManager.Instance.highScoresGame) return;
        int chance = UnityEngine.Random.Range(0,101);
        if (generateChance < chance) return;

        for (int i = 0; i < count; i++) {
            Vector3 spawnPos = new Vector3(transform.position.x + UnityEngine.Random.Range(GameManager.Instance.startSpawnPos.x,GameManager.Instance.endSpawnPos.x), GameManager.Instance.startSpawnPos.y + UnityEngine.Random.Range(-1.5f,1.5f));
            GameObject newObject = Instantiate(generatingObject, spawnPos ,Quaternion.identity);
            GameManager.objects.Add(newObject.gameObject);
        }
    }

    void RestartGame() {
        List<GameObject> objectsToDestroy = new List<GameObject>();
        foreach (GameObject obj in GameManager.objects) { 
            if (obj != null) objectsToDestroy.Add(obj);
        }
        foreach (GameObject obj in objectsToDestroy) {
            Destroy(obj.gameObject);
            GameManager.objects.Remove(obj);
        }
    }
}

[Serializable]
public class GeneratingObject {
    public GameObject generatingObject;
    public int count = 1;
    public int needScores;
    public int maxNeedScores = 2000;
    public int generateChance = 100;
}
