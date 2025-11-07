using System.Collections;
using System.Collections.Generic;
using UnityEngine;
internal class Map
{
    GameObject area;
    List<Transform> spawnPoints;
    List<GameObject> walls;

    public Map(GameObject area, List<Transform> checkpoints, List<GameObject> walls)
    {
        this.area = area;
        this.spawnPoints = checkpoints;
        this.walls = walls;
    }

    public List<Transform> GetSpawnPoints()
    {
        return spawnPoints;
    }
}
public class GameLogic : MonoBehaviour
{
    [SerializeField] ulong round = 1;



    [Header("Map")]
    [SerializeField] GameObject mapHolder;
    [SerializeField] List<Map> maps;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
