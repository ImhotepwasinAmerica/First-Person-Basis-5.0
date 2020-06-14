using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorProbe : MonoBehaviour
{
    public enum Directions { X, Y, Z };
    public Directions orientation;
    public GameObject mirror;
    public float render_distance;

    private float offset;
    private Vector3 probePos;
    private GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");

        if (Vector3.Distance(mirror.transform.position, camera.transform.position) < render_distance)
        {
            if (orientation == Directions.X)
            {
                offset = mirror.transform.position.x - Camera.main.transform.position.x;

                probePos.x = mirror.transform.position.x + offset;
                probePos.y = camera.transform.position.y;
                probePos.z = camera.transform.position.z;
            }
            else if (orientation == Directions.Y)
            {
                offset = mirror.transform.position.y - Camera.main.transform.position.y;

                probePos.x = camera.transform.position.x;
                probePos.y = mirror.transform.position.y + offset;
                probePos.z = camera.transform.position.z;
            }
            else if (orientation == Directions.Z)
            {
                offset = mirror.transform.position.z - Camera.main.transform.position.z;

                probePos.x = camera.transform.position.x;
                probePos.y = camera.transform.position.y;
                probePos.z = mirror.transform.position.z + offset;
            }
        }
        else
        {
            probePos = mirror.transform.position;
        }


        transform.position = probePos;
    }
}
