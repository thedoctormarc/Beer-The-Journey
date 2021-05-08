using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<Car_Script> cars;

    BoxCollider[] triggers;

    public float spawnTime = 5f;
    private float auxSpawnTime;
    void Start()
    {
        triggers = GetComponentsInChildren<BoxCollider>();


        auxSpawnTime = spawnTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        auxSpawnTime -= Time.deltaTime;

        if(auxSpawnTime <=0.0f)
        {
            int num = Random.Range(0, 7);

            Instantiate(cars[num], triggers[0].transform);

            auxSpawnTime = spawnTime;
        }
        
    }


    
}
