using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnMeteor : MonoBehaviour
{

    public GameObject vfx;
    public Transform startP;
    public Transform endP;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 startPos = startP.position;
        GameObject objVFX = Instantiate(vfx, startPos,Quaternion.identity) as GameObject;
        Vector3 endPos = endP.position;
        RotateTo(objVFX, endPos);
    }

    // Update is called once per frame
    void RotateTo(GameObject obj, Vector3 destination)
    {
        Vector3 direction = destination - obj.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }
}
