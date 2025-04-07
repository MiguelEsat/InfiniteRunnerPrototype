using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    [SerializeField] private float speed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float move = speed * Time.deltaTime;
        transform.position += new Vector3(move,0,0);
    }
}
