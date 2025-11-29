using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Transform fontBar;


    public void SetProgress(float currentValue, float maxValue)
    {
        var ratio = Mathf.Clamp01(currentValue / maxValue);
        fontBar.localScale = new Vector3(ratio, 1f, 1f);
    }

}
