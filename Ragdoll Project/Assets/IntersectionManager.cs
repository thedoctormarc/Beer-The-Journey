using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionManager : MonoBehaviour
{
    public static IntersectionManager instance;


    public delegate void OnSwap(GameObject[] activeCurves, GameObject[] inActiveCurves);
    public event OnSwap OnSwapEvent;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSwapIntersection (GameObject[] activeCurves, GameObject[] inActiveCurves)
    {
        OnSwapEvent(activeCurves, inActiveCurves);
    }
}
