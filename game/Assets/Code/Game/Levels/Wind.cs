using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;


    void Start()
    {
        this.StartCoroutine(this.Flying());
    }

    private IEnumerator Flying()
    {
        while (true)
        {
            while (this.transform.position.x < this.EndPoint.position.x)
            {
                this.transform.position += new Vector3(Time.deltaTime, 0, 0);
                this.transform.eulerAngles += new Vector3(0, Time.deltaTime * 10, 0);
                yield return null;
            }

            while (this.transform.position.x > this.StartPoint.position.x)
            {
                this.transform.position -= new Vector3(Time.deltaTime, 0, 0);
                this.transform.eulerAngles -= new Vector3(0, Time.deltaTime * 10, 0);
                yield return null;
            }
        }
    }
}
