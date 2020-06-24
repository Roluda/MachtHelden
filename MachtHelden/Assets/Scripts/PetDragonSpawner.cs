using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetDragonSpawner : MonoBehaviour
{

    [SerializeField]
    PlayerController observedPlayer;
    [SerializeField]
    float respawnTime = 1;
    [SerializeField]
    PetDragon dragonPrefab;
    [SerializeField]
    float minRadius;
    [SerializeField]
    float maxRadius;
    [SerializeField]
    float amplitudeMultiplier;
    [SerializeField]
    float speedMultiplier;

    float spawnTimer = 1;
    int dragonsOut = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <0 && dragonsOut < MaximumDragons(observedPlayer.powerLevel)){
            dragonsOut++;
            spawnTimer = respawnTime + Random.Range(0, respawnTime);
            Vector3 spawnPos = observedPlayer.transform.position + new Vector3(0, 1, -Random.Range(minRadius, maxRadius));
            PetDragon newDragon = Instantiate(dragonPrefab, spawnPos, Quaternion.identity, observedPlayer.transform);
            newDragon.amplitude *= Vector3.Distance(newDragon.transform.position, observedPlayer.transform.position) * amplitudeMultiplier;
            newDragon.degreePerSecond *= Vector3.Distance(newDragon.transform.position, observedPlayer.transform.position) * speedMultiplier;
            newDragon.target = observedPlayer.transform;
        }
    }

    Vector3 SpawnPosition() {
        return observedPlayer.transform.position + new Vector3(0,1,-Random.Range(minRadius, maxRadius));
    }

    int MaximumDragons(int powerLevel) {
        return powerLevel;
    }
}
