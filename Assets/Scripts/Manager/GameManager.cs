using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject drone;

    [SerializeField]
    private Vector3 spawnOffset = new Vector3(0, 1, 10);

    [SerializeField]
    private Vector3 buildingScaling = new Vector3(2, 2, 2);

    [SerializeField]
    private GameObject buildingsContainer;

    [SerializeField]
    private List<GameObject> buildingsPrefabs = new List<GameObject>();

    [SerializeField]
    private int distanceBetweenBuildings = 30;

    private List<GameObject> buildings = new List<GameObject>();

    private int currentBuildingId = 0;

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
            generatedObject.transform.localScale = buildingScaling;
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

        drone.transform.position = buildings[currentBuildingId].transform.position + spawnOffset;
    }
}