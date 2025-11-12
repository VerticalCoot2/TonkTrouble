using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerStat : MonoBehaviour
{
    [SerializeField] string playerName;
    [SerializeField] int wins;
    [SerializeField] int kills;
    [SerializeField] int deaths;

    public PlayerStat(byte playerIndex)
    {
        playerName = $"Player{playerIndex}";
        wins = 0;
        deaths = 0;
        kills = 0;
    }
    public void Died()
    {
        deaths++;
    }
  
    public void Won()
    {
        wins++;
    }
    public int[] GetStats()
    {
        int[] stats = { wins, kills, deaths };
        return stats;
    }
}
