using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Accessibility;
[System.Serializable]
internal class Map
{
    [SerializeField] GameObject area;
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] List<Transform> walls = new List<Transform>();

    public Map(GameObject area, Transform spawnpointHolder, Transform wallHolder)
    {
        this.area = area; //EZ maga a map holder cucc, amiben vannak a falak, és a csekpointok
        for (int i = 0; i < spawnpointHolder.childCount; i++)
        {
            spawnPoints.Add(spawnpointHolder.GetChild(i));
        }
        for(int i = 0; i < wallHolder.childCount; i++)
        {
            walls.Add(wallHolder.GetChild(0).transform);
        }
    }

    public List<Transform> GetSpawnPoints()
    {
        return spawnPoints;
    }
}

public class GameLogic : MonoBehaviour
{
    [SerializeField] ulong round = 0;



    [Header("Map")]
    [SerializeField] GameObject mapHolder;
    [SerializeField] List<Map> maps = new List<Map>();

    [Header("Players")]
    [SerializeField] Transform playerHolder;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] List<GameObject> players = new List<GameObject>();
    public bool roundOngoing = true;

    void Awake()
    {
        if (mapHolder.transform.childCount > 0)
        {
            for (int i = 0; i < mapHolder.transform.childCount; i++)
            {
                GameObject currentMap = mapHolder.transform.GetChild(i).gameObject;
                Transform mapsSpawnPoints = currentMap.transform.Find("SpawnPoints").transform;
                Transform mapsWalls = currentMap.transform.Find("Walls").transform;
                maps.Add(new Map(currentMap, mapsSpawnPoints, mapsWalls));
                //Debug.Log(currentMap.transform.Find("Walls"));
            }
        }

        for(int i = 0; i < playerHolder.childCount; i++)
        {
            GameObject THIS = Instantiate(playerPrefab, playerHolder);
            var thisScript = THIS.GetComponent<PlayerController>();
            thisScript.parent = playerHolder.gameObject;
            thisScript.index = (byte)i;
            thisScript.playerStat = new PlayerStat((byte)i);
            thisScript.gameLogic = gameObject.GetComponent<GameLogic>();
        }
    }
    private void Start()
    {
        NewMap();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            NewMap();
        }
    }

    //public void GetDeath(byte playerIndex, bool suicide)
    //{

    //}

    public IEnumerator SomeoneDied()
    {
        yield return new WaitForSeconds(3);
    }

    public void NewMap()
    {
        round++;
        for(int i = 0; i < mapHolder.transform.childCount; i++)
        {
            mapHolder.transform.GetChild(i).gameObject.SetActive(false);
        }
        List<Transform> spawnPoints;
        
        if(maps.Count > 0)
        {

            int randomMapIndex = UnityEngine.Random.Range(0, mapHolder.transform.childCount-1);
            mapHolder.transform.GetChild(randomMapIndex).gameObject.SetActive(true);
            spawnPoints = maps[randomMapIndex].GetSpawnPoints();
            byte playerPlaced = 0;
            int occupiedIndex = int.MinValue;
            while (playerPlaced != 2)
            {
                int randomSpawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Count - 1);
                Debug.Log(playerPlaced);
                if(occupiedIndex != randomSpawnPointIndex)
                {
                    //if(players.Count <= 2)
                    //{
                    //    for(int i = 0; i < playerHolder.childCount; i++)
                    //    {
                    //        Destroy(playerHolder.transform.GetChild(i).gameObject);
                    //    }

                        players.Clear();

                        GameObject player = Instantiate(playerPrefab, playerHolder);
                        player.GetComponent<PlayerController>().SetIndex(playerPlaced);
                        players.Add(player);
                    //}

                    Debug.Log(playerPlaced);
                    players[playerPlaced].transform.position = spawnPoints[randomSpawnPointIndex].position;
                    playerPlaced++;
                    occupiedIndex = randomSpawnPointIndex;

                }
            }
        }
    }
}
