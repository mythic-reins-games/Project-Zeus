using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorBar : MonoBehaviour
{

    [SerializeField] private GameObject foreground;

    private float updateTimeSeconds = 0.35f;

    // Start is called before the first frame update
    void Start()
    {
        foreground.GetComponent<Image>().fillAmount = 1.0f;
    }

    public void SetPercent(float percent)
    {
        StartCoroutine(GradualizePercentChange(percent));
    }

    private IEnumerator GradualizePercentChange(float percent)
    {
        float originalPercent = foreground.GetComponent<Image>().fillAmount;
        float elapsed = 0f;

        while (elapsed < updateTimeSeconds)
        {
            elapsed += Time.deltaTime;
            foreground.GetComponent<Image>().fillAmount = Mathf.Lerp(originalPercent, percent, elapsed / updateTimeSeconds);
            yield return null;
        }
        foreground.GetComponent<Image>().fillAmount = percent;
        yield break;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
        transform.eulerAngles = new Vector3(
            0,
            transform.eulerAngles.y,
            0
        );
    }
}
