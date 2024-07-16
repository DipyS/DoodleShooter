using System.Collections.Generic;
using UnityEngine;

public class ProgrammSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnObjects;
    public void Spawn() {
        for (int i = 0; i < spawnObjects.Count; i++) {
            Instantiate(spawnObjects[i], transform.position, Quaternion.identity);
        }
    }
}
