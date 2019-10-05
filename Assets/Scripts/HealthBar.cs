using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            0,
            transform.eulerAngles.z
        );
    }
}
