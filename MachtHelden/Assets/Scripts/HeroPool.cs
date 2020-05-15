using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPool : MonoBehaviour
{
    public static HeroPool Instance;
    [SerializeField]
    public HeroData[] heroes;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
