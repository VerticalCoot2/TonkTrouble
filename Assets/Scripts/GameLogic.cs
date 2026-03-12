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
    public static ulong round = 0;
    [SerializeField] Color[] playerColors = { new Color(211, 73, 42), new Color(255, 0, 255) };


    [Header("Map")]
    [SerializeField] GameObject mapHolder;
    [SerializeField] List<Map> maps = new List<Map>();

    [Header("Players")]
    [SerializeField] Transform bulletHolder;
    [SerializeField] Transform playerHolder;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] List<GameObject> players = new List<GameObject>();

    [SerializeField] Statistics statHolder;
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
            PlayerController playerControllerScript = THIS.GetComponent<PlayerController>();            
            playerControllerScript.index = (byte)i;

            playerControllerScript.parent = playerHolder.gameObject;

            //Debug.Log(this.gameObject.GetComponent<GameLogic>());
            playerControllerScript.gameLogic = this.gameObject.GetComponent<GameLogic>();
        }
    }
    private void Start()
    {
        NewMap();
        //Debug.Log(stats.AddPlayerShit(playerColors[0], playerColors[1]));
        
        statHolder.AddPlayerShit(playerColors[0], playerColors[1]);
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
        Debug.Log("New rounding...");
        NewMap();
    }

    public void NewMap()
    {
        round++;
        for(int i = 0; i < mapHolder.transform.childCount; i++)
        {
            mapHolder.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < playerHolder.transform.childCount; i++)
        {
            Destroy(playerHolder.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < bulletHolder.childCount; i++)
        {
            //bulletHolder.transform.GetChild(i).GetComponent<Bullet>().Reload();
            Destroy(bulletHolder.transform.GetChild(i).gameObject);
        }

        List<Transform> spawnPoints;
        
        if(maps.Count > 0)
        {
            players.Clear();

            int randomMapIndex = UnityEngine.Random.Range(0, mapHolder.transform.childCount);
            mapHolder.transform.GetChild(randomMapIndex).gameObject.SetActive(true);
            spawnPoints = new List<Transform>(maps[randomMapIndex].GetSpawnPoints());
            for (int i = 0; i < 2; i++)
            {
                GameObject player = Instantiate(playerPrefab, playerHolder);
                PlayerController playerControllerScript = player.GetComponent<PlayerController>();
                playerControllerScript.index = (byte)i;

                playerControllerScript.parent = playerHolder.gameObject;
                playerControllerScript.gameLogic = this;
                playerControllerScript.bulletHolder = bulletHolder;
                playerControllerScript.player_ONE_Color = playerColors[0];
                playerControllerScript.player_TWO_Color = playerColors[1];
                playerControllerScript.stats = statHolder;

                int SPI = UnityEngine.Random.Range(0, spawnPoints.Count);

                player.transform.position = spawnPoints[SPI].position;
                

                players.Add(player);
                spawnPoints.RemoveAt(SPI);
            }
        }
    }
}
