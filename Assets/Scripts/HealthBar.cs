using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private GameObject foreground;

    // Start is called before the first frame update
    void Start()
    {
        foreground.GetComponent<Image>().fillAmount = 1.0f;
    }

    public void TakeDamage()
    {
        foreground.GetComponent<Image>().fillAmount -= 0.45f;
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
