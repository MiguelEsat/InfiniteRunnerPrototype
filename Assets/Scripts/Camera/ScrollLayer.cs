using Unity.VisualScripting;
using UnityEngine;

public class ScrollLayer : MonoBehaviour
{

    [SerializeField] private Transform camera;
    [SerializeField] private float parallaxMulti = 0.5f;

    private float spriteWidth;
    private Vector3 lastCamPos;


    private Transform[] backgrounds = new Transform[2];


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

        if (camera == null) camera = Camera.main.transform;
        lastCamPos = camera.position;

        spriteWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;




    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaMovement = camera.position - lastCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxMulti, 0, 0);
        lastCamPos= camera.position;

        foreach(var bg in backgrounds)
        {
            float camDistance = camera.position.x - bg.position.x;
            if (Mathf.Abs(camDistance) >= spriteWidth)
            {
                float offset = (camDistance>0) ? spriteWidth * 2f : -spriteWidth * 2f;
                bg.position += new Vector3(offset, 0, 0);

            }


        }




    }
}
