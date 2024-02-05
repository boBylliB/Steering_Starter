using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Kinematic> ATeam;
    public Kinematic targetA;
    public GameObject goalA;
    public string tagA = "Player";
    public Material matA;

    public List<Kinematic> BTeam;
    public Kinematic targetB;
    public GameObject goalB;
    public string tagB = "Enemy";
    public Material matB;

    public List<StandIn> standIns;
    public int standInSelectLock = -1;
    int nextID = 0;

    public bool debugEnabled = false;

    public List<Kinematic> getPlayers(bool teamA)
    {
        return teamA ? ATeam : BTeam;
    }
    public GameObject getGoal(bool teamA)
    {
        return teamA ? goalB : goalA;
    }
    public void updateTarget(Kinematic player, bool teamA)
    {
        if (teamA)
            targetA = player;
        else
            targetB = player;
    }
    public Material getMaterial(bool teamA)
    {
        return teamA ? matA : matB;
    }
    public string getTag(bool teamA)
    {
        return teamA ? tagA : tagB;
    }
    public int getID()
    {
        return nextID++;
    }
    public void startGame()
    {
        while (standIns.Count > 0)
        {
            StandIn standIn = standIns[0];
            standIns.RemoveAt(0);
            standIn.spawnPlayer(debugEnabled);
        }
    }
}
