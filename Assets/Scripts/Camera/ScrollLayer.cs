using Unity.VisualScripting;
using UnityEngine;

public class ScrollLayer : MonoBehaviour
{

    [SerializeField] private Transform cam;
    [SerializeField] private float parallaxMulti = 0.5f;

    private float spriteWidth;
    private Vector3 lastCamPos;


    [SerializeField] private Transform[] backgrounds = new Transform[2];


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(transform.childCount != 2)
        {
            Debug.LogError("ParallaxError");
            return;
        }
        for (int i = 0; i < 2; i++)
        {
            backgrounds[i] = transform.GetChild(i);
        }

        if (cam == null) cam = Camera.main.transform;
        lastCamPos = cam.position;

        spriteWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;




    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaMovement = new Vector3(0.001f, 0.0f, 0.0f);
        transform.position += new Vector3(deltaMovement.x * parallaxMulti, 0, 0);


        foreach(var bg in backgrounds)
        {
            if (cam)
            {
                float camDistance = cam.position.x - bg.position.x;
                if (Mathf.Abs(camDistance) >= spriteWidth)
                {
                    float offset = (camDistance>0) ? spriteWidth * 2f : -spriteWidth * 2f;
                    bg.position += new Vector3(offset, 0, 0);
                }
            }
        }
    }
}
