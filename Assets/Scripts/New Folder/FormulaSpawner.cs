using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormulaSpawner : MonoBehaviour
{
    public static FormulaSpawner Instance;

    public GameObject formulaPrefab;
    public Transform[] spawnPoints;

    public Transform[] spawnPointsLevel2;
    public Transform[] spawnPointsLevel3;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnComponents(string[] components, int level)
    {
        List<Transform> availableSpawnPoints = GetSpawnPointsForLevel(level);

        foreach (Transform spawnPoint in availableSpawnPoints)
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

    private List<Transform> GetSpawnPointsForLevel(int level)
    {
        switch (level)
        {
            case 0:
                return new List<Transform>(spawnPoints);
            case 1:
                return new List<Transform>(spawnPointsLevel2);
            case 3:
                return new List<Transform>(spawnPointsLevel3);
            default:
                Debug.LogWarning("Nivel no válido, usando puntos de spawn del nivel 1 por defecto.");
                return new List<Transform>(spawnPoints);
        }
    }

}
