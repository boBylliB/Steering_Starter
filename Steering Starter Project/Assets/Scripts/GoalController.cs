using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public MeshRenderer mr;
    public bool isPlayerA;
    public Material matA;
    public Material matB;

    // Start is called before the first frame update
    void Start()
    {
        if (isPlayerA)
            mr.material = matA;
        else
            mr.material = matB;
    }
}
