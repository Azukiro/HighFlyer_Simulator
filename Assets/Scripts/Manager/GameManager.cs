using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject drone;

    [SerializeField]
    Vector3 spawnOffset = new Vector3(0, 1, 10);

    [SerializeField]
    GameObject buildingsContainer;

    [SerializeField]
    List<GameObject> buildingsPrefabs = new List<GameObject>();

    [SerializeField]
    int distanceBetweenBuildings = 30;

    List<GameObject> buildings = new List<GameObject>();

    int currentBuildingId = 0;

    // Singleton pattern
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    private void Start()
    {
        int x = 0;
        foreach (GameObject buildingPrefab in buildingsPrefabs)
        {
            GameObject generatedObject = Instantiate(buildingPrefab, new Vector3(x, 0, 0), Quaternion.identity);
            generatedObject.transform.parent = buildingsContainer.transform;
            buildings.Add(generatedObject);
            x += distanceBetweenBuildings;
        }

        if (0 < buildings.Count)
            LaunchDrone();
    }

    public void LaunchDrone()
    {
        if (!(0 <= currentBuildingId && currentBuildingId < buildings.Count))
            return;

        Debug.Log("Launch drone...");
        drone.transform.position = buildings[currentBuildingId].transform.position + spawnOffset;
    }

}
