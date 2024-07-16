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
            GenerateObject(objects[i].generatingObject,objects[i].needScores,objects[i].maxNeedScores);
        }
     }

    public void GenerateObject(GameObject generatingObject,int needScores,int maxNeedScores) {
        if (needScores > GameManager.Instance.highScoresGame || maxNeedScores <= GameManager.Instance.highScoresGame) return;


        GameObject newObject = Instantiate(generatingObject, Camera.main.transform);
    }
}

[Serializable]
public class GeneratingBoss {
    public GameObject generatingObject;
    public int needScores;
    public int maxNeedScores = 2000;
}
