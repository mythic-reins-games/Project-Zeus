using UnityEngine;
using System.Collections;

public class CombatCamera : MonoBehaviour
{

    private float updateTimeSeconds = 0.25f;

    public void RotateLeft()
    {
        transform.Rotate(Vector3.up, 90, Space.Self);
    }

    public void RotateRight()
    {
        transform.Rotate(Vector3.up, -90, Space.Self);
    }

    void MouseScroll()
    {
        Vector3 CameraSpeed = new Vector3(0f, 0f, 0f);
        Vector3 CurrentMousePosition = Input.mousePosition;
        if (CurrentMousePosition.x >= Screen.width - 20)
        {
            CameraSpeed.x = Time.deltaTime * 10f;
        }
        if (CurrentMousePosition.x <= 20)
        {
            CameraSpeed.x = Time.deltaTime * -10f;
        }
        if (CurrentMousePosition.y >= Screen.height - 20)
        {
            CameraSpeed.z = Time.deltaTime * 10f;
        }
        if (CurrentMousePosition.y <= 20)
        {
            CameraSpeed.z = Time.deltaTime * -10f;
        }
        transform.Translate(CameraSpeed, Space.World);

        // Clamp the camera position so it doesn't go too far away from the grid.
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -5f, 5f),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -10f, 1f)
        );
    }

    void Update()
    {
        MouseScroll();
    }

    private IEnumerator GradualizeMove(Vector3 loc)
    {
        Vector3 initial = transform.position;
        float elapsed = 0f;
        while (elapsed < updateTimeSeconds)
        {
            elapsed += Time.deltaTime;
            float x = Mathf.SmoothStep(initial.x, loc.x, elapsed / updateTimeSeconds);
            float z = Mathf.SmoothStep(initial.z, loc.z, elapsed / updateTimeSeconds);
            transform.position = new Vector3(x, initial.y, z);
            yield return null;
        }
        transform.position = loc;
        yield break;
    }

    public void ZoomNear(CombatController target)
    {
        Vector3 newTarget = target.transform.position;
        newTarget.z -= 3.5f;
        newTarget.y = transform.position.y;
        StartCoroutine(GradualizeMove(newTarget));
    }

}
