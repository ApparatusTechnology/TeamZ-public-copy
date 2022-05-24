using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBot : MonoBehaviour
{
    private Camera mainCamera;

    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        this.mainCamera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += Vector3.right * (Time.deltaTime * this.Speed);
    }
}
