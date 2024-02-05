using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandIn : MonoBehaviour
{
    GameManager gm;
    Camera cam;

    public bool teamA;

    public GameObject indicator;
    public Material targetMat;
    public Material attackMat;
    public Material defendMat;

    public Player.playerTypes type;
    public GameObject playerPrefab;

    bool selected = false;
    int ID;

    Vector3 mouseOffset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        ID = gm.getID();
        gm.standIns.Add(this);
        cam = FindObjectOfType<Camera>();

        GetComponent<MeshRenderer>().material = gm.getMaterial(teamA);
        switch (type)
        {
            case Player.playerTypes.target:
                indicator.GetComponent<MeshRenderer>().material = targetMat;
                break;
            case Player.playerTypes.attacker:
                indicator.GetComponent<MeshRenderer>().material = attackMat;
                break;
            case Player.playerTypes.defender:
                indicator.GetComponent<MeshRenderer>().material = defendMat;
                break;
            default:
                Debug.Log("Unknown type!");
                break;
        }
    }

    void OnMouseEnter()
    {
        if (gm.standInSelectLock == -1 && type != Player.playerTypes.target)
        {
            gm.standInSelectLock = ID;
            selected = true;
        }
    }
    void OnMouseOver()
    {
        if (selected && Input.GetMouseButtonDown(0))
        {
            mouseOffset = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.y)) - transform.position;
        }
    }
    void Update()
    {
        if (selected && Input.GetMouseButton(0))
        {
            transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.y)) + mouseOffset;
        }
        else if (!Input.GetMouseButton(0))
        {
            if (gm.standInSelectLock == ID)
                gm.standInSelectLock = -1;
            selected = false;
        }
    }
    void OnMouseExit()
    {
        if (selected && Input.GetMouseButton(0)) return;

        if (gm.standInSelectLock == ID)
            gm.standInSelectLock = -1;
        selected = false;
    }

    public void spawnPlayer()
    {
        GameObject newObject = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        Player player = newObject.GetComponent<Player>();
        player.teamA = teamA;
        player.type = type;
        if (type == Player.playerTypes.target)
            gm.updateTarget(player, teamA);
        Object.Destroy(gameObject);
    }
}
