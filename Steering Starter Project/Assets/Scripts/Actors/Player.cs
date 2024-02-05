using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Kinematic
{
    Pursue myMoveType;
    AggressiveCollisionAvoidance myEnemyAvoidType;
    ObstacleAvoidance myWallAvoidType;
    Separation mySeparateType;
    LookWhereGoing myRotateType;

    public float separateStrength = 1f;
    public float collAvoidStrength = 1f;
    public float wallAvoidStrength = 1f;

    CollisionHandler collHandler;

    GameManager gm;

    public bool teamA;

    public GameObject indicator;
    public Material targetMat;
    public Material attackMat;
    public Material defendMat;

    public enum playerTypes
    {
        target, defender, attacker
    };
    public playerTypes type;

    // Minimum distance to a wall
    public float avoidDist = 30f;
    // The distance to look for collisions
    public float lookAhead = 10f;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        if (teamA)
            gm.ATeam.Add(this);
        else
            gm.BTeam.Add(this);
        GetComponent<MeshRenderer>().material = gm.getMaterial(teamA);
        tag = gm.getTag(teamA);

        myMoveType = new Pursue();
        myMoveType.character = this;
        myMoveType.target = type == playerTypes.target ? gm.getGoal(teamA) : null;

        myEnemyAvoidType = new AggressiveCollisionAvoidance();
        myEnemyAvoidType.character = this;
        myEnemyAvoidType.targets = gm.getPlayers(!teamA);

        myWallAvoidType = new ObstacleAvoidance();
        myWallAvoidType.character = this;
        myWallAvoidType.avoidDist = avoidDist;
        myWallAvoidType.lookAhead = lookAhead;
        myWallAvoidType.ignoredTags = new List<string>();
        myWallAvoidType.ignoredTags.Add(gm.tagA);
        myWallAvoidType.ignoredTags.Add(gm.tagB);

        mySeparateType = new Separation();
        mySeparateType.character = this;
        mySeparateType.targets = gm.getPlayers(teamA);

        myRotateType = new LookWhereGoing();
        myRotateType.character = this;
        myRotateType.target = myTarget;

        collHandler = new CollisionHandler();
        collHandler.character = this;
        collHandler.tagA = gm.tagA;
        collHandler.tagB = gm.tagB;
        collHandler.isPlayerA = teamA;

        switch (type)
        {
            case playerTypes.target:
                indicator.GetComponent<MeshRenderer>().material = targetMat;
                gm.updateTarget(this, teamA);
                break;
            case playerTypes.attacker:
                indicator.GetComponent<MeshRenderer>().material = attackMat;
                break;
            case playerTypes.defender:
                indicator.GetComponent<MeshRenderer>().material = defendMat;
                break;
            default:
                Debug.Log("Unknown type!");
                break;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        myEnemyAvoidType.targets = gm.getPlayers(!teamA);
        mySeparateType.targets = gm.getPlayers(teamA);
        steeringUpdate = new SteeringOutput();
        if (!collHandler.collided)
        {
            switch (type)
            {
                case playerTypes.attacker:
                    myMoveType.target = teamA ? gm.targetB.gameObject : gm.targetA.gameObject;
                    break;
                case playerTypes.defender:
                    float minDist = float.MaxValue;
                    foreach (Kinematic player in teamA ? gm.BTeam : gm.ATeam)
                    {
                        float dist = (player.transform.position - (teamA ? gm.targetA : gm.targetB).transform.position).magnitude;
                        if (dist < minDist)
                        {
                            minDist = dist;
                            myMoveType.target = player.gameObject;
                        }
                    }
                    break;
            }
            steeringUpdate.linear = myMoveType.getSteering().linear + wallAvoidStrength * myWallAvoidType.getSteering().linear + separateStrength * mySeparateType.getSteering().linear;
            if (type == playerTypes.target)
                steeringUpdate.linear += collAvoidStrength * myEnemyAvoidType.getSteering().linear;
            steeringUpdate.angular = myRotateType.getSteering().angular;
        }
        else
        {
            GetComponent<MeshRenderer>().material = attackMat;
            steeringUpdate.linear = Vector3.zero;
            steeringUpdate.angular = 0;
            tag = "Untagged";
            if (teamA)
                gm.ATeam.Remove(this);
            else
                gm.BTeam.Remove(this);
        }
        base.Update();
        // Simple double check to make sure the y level is still 0
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public void OnCollisionEnter(Collision collision)
    {
        collHandler.checkCollision(collision);
        if (type == playerTypes.target && collision.gameObject.CompareTag("Finish"))
        {
            FindObjectOfType<UIController>().win(teamA);
            Time.timeScale = 0;
        }
    }
}
