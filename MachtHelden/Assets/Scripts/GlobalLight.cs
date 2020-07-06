using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLight : MonoBehaviour
{
    public static GlobalLight Instance;

    [SerializeField]
    Color normalColor;
    [SerializeField]
    Color dangerColor;
    [SerializeField]
    Light light;
    // Start is called before the first frame update
    void Start() {
        if (Instance != null) {
            DestroyImmediate(this);
        } else {
            Instance = this;
        }
    }

    public void DangerMode() {
        light.color = dangerColor;
    }

    public void NormalMode() {
        light.color = normalColor;
    }
}
