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
        transform.position = new Vector3(loc.x, initial.y, loc.z); ;
        yield break;
    }

    public void ZoomNear(CombatController target)
    {
        Vector3 newTarget = target.transform.position;
        newTarget.z -= 5.5f;
        StartCoroutine(GradualizeMove(newTarget));
    }

}
