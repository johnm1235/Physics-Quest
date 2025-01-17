using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormulaSpawner : MonoBehaviour
{
    public static FormulaSpawner Instance;

    public GameObject formulaPrefab;
    public Transform[] spawnPoints;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnComponents(string[] components)
    {
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        foreach (Transform spawnPoint in spawnPoints)
        {
            // Limpia los objetos anteriores
            foreach (Transform child in spawnPoint)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (string component in components)
        {
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("No hay suficientes puntos de spawn disponibles para todos los componentes.");
                break;
            }

            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform randomPoint = availableSpawnPoints[randomIndex];
            availableSpawnPoints.RemoveAt(randomIndex);

            GameObject spawned = Instantiate(formulaPrefab, randomPoint.position, Quaternion.identity);
            spawned.GetComponent<FormulaComponent>().value = component;
        }
    }

}
