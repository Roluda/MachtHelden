using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    [SerializeField]
    FireZone nearbyZone;
    [SerializeField]
    PetDragon dragon;
    [SerializeField]
    float clickCooldown = 1f;
    [SerializeField]
    int extinguishValue = 5;
    [SerializeField]
    LayerMask clickLayer = default;
    float timer = 0;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit info, 100, clickLayer)) {
                if(info.collider == GetComponent<Collider>()) {
                    FireEvent();
                }
            }
        }
        timer += Time.deltaTime;
    }

    void FireEvent() {
        if (nearbyZone.mainHeroNearby) {
            if (timer > clickCooldown) {
                Debug.Log("FireEvent");
                timer = 0;
                nearbyZone.LowerFire(extinguishValue);
                dragon.TakeHit();
            }
        }
    }
}
