using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Start_Trigger : MonoBehaviour
{
    public GameObject Beer_Image;
    public GameObject Shop_Image;

    public float Time_Showing_Image = 5.0f;

    private float Timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Beer_Image.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {

        //Beer
        RectTransform Beer_Rect = Beer_Image.GetComponent<RectTransform>();
        if (Beer_Rect.localScale.x < 1 && Beer_Image.activeSelf && Timer < Time_Showing_Image)
        {
            Beer_Rect.localScale = new Vector3(Beer_Rect.localScale.x + Time.deltaTime, Beer_Rect.localScale.y + Time.deltaTime, Beer_Rect.localScale.z + Time.deltaTime);
            return;
        }

        Timer += Time.deltaTime;

        if (Timer < Time_Showing_Image && Beer_Image.activeSelf)
            return;

        if (Timer >= Time_Showing_Image && Beer_Image.activeSelf)
        {
            Beer_Rect.localScale = new Vector3(Beer_Rect.localScale.x - Time.deltaTime, Beer_Rect.localScale.y - Time.deltaTime, Beer_Rect.localScale.z - Time.deltaTime);
            if (Beer_Rect.localScale.x <= 0)
            {
                Beer_Image.SetActive(false);
                Timer = 0.0f;
                Shop_Image.SetActive(true);

            }

            return;

        }

        //Shop
        RectTransform Shop_Rect = Shop_Image.GetComponent<RectTransform>();
        if (Shop_Rect.localScale.x < 1 && Shop_Image.activeSelf && Timer < Time_Showing_Image)
        {
            Shop_Rect.localScale = new Vector3(Shop_Rect.localScale.x + Time.deltaTime, Shop_Rect.localScale.y + Time.deltaTime, Shop_Rect.localScale.z + Time.deltaTime);
            return;
        }

        Timer += Time.deltaTime;

        if (Timer < Time_Showing_Image && Shop_Image.activeSelf)
            return;

        if (Timer >= Time_Showing_Image && Shop_Image.activeSelf)
        {
            Shop_Rect.localScale = new Vector3(Shop_Rect.localScale.x - Time.deltaTime, Shop_Rect.localScale.y - Time.deltaTime, Shop_Rect.localScale.z - Time.deltaTime);
            if (Shop_Rect.localScale.x <= 0)
            {
                Shop_Image.SetActive(false);
                Timer = 0.0f;
                enabled = false;

            }
        }
    }
}
