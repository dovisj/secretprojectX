using System.Collections;
using System.Collections.Generic;
using Plants;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField]
    private Plant[] currentStock;
    [SerializeField]
    private int maxStock;

    private float stockChangeTimer;

    void GenerateStock()
    {
        
    }
}
