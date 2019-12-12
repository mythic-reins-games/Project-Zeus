using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationPopupOk : MonoBehaviour
{
    public void Clicked()
    {
        Destroy(transform.parent.gameObject);
    }
}
