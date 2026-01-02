using System.Collections;
using UnityEngine;

public sealed class VibrationService : MonoBehaviour
{
    public static VibrationService I { get; private set; }

    private void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Light()
    {
        StartCoroutine(VibrationPattern(1, 0f));
    }

    public void Medium()
    {
        StartCoroutine(VibrationPattern(2, 0.08f));
    }

    public void Heavy()
    {
        StartCoroutine(VibrationPattern(3, 0.1f));
    }

    private IEnumerator VibrationPattern(int count, float delay)
    {
        if (!SettingsService.I.Data.VibrationEnabled)
            yield break;

#if UNITY_ANDROID || UNITY_IOS
        for (int i = 0; i < count; i++)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(delay);
        }
#endif
    }
}