using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;


public class Player
{
    public uint kills, deaths;
    public Color color;
    
    public Player(Color playerColor)
    {
        kills = 0;
        deaths = 0;
        color = playerColor;
    }

    public void Killed()
    {
        kills++;
    }

    public void Died()
    {
        deaths++;
    }
}
public class Statistics : MonoBehaviour
{
    [SerializeField] public List<Player> players = new List<Player>();

    [Header("Player1")]
    [SerializeField] RawImage p1Raw;
    [SerializeField] TMP_Text p1KillText;
    [SerializeField] TMP_Text p1DeathText;

    [Header("Player2")]
    [SerializeField] RawImage p2Raw;
    [SerializeField] TMP_Text p2KillText;
    [SerializeField] TMP_Text p2DeathText;

    void Start()
    {

        
    }

    public void AddPlayerShit(Color c1,Color c2)
    {
        //Debug.LogWarning($"Elindult\nc1: {c1}\nc2: {c2}");
        players.Add(new Player(c1));
        players.Add(new Player(c2));
        p1Raw.color = c1;
        p2Raw.color = c2;
    }

    // Update is called once per frame
    void Update()
    {
        if (players.Count == 2)
        {
            p1KillText.text = "Kills: " + players[0].kills;
            p2KillText.text = "Kills: " + players[1].kills;

            p1DeathText.text = "Deaths: " + players[0].deaths;
            p2DeathText.text = "Deaths: " + players[1].deaths;
        }
    }
}
