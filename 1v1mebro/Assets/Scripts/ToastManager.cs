using UnityEngine;
using TMPro;
using System.Collections;

public class ToastManager : MonoBehaviour
{
    public TextMeshProUGUI toastText;
    public float displayDuration = 2f;

    private Coroutine toastCoroutine;

    void Awake()
    {
        if (toastText != null)
        {
            toastText.gameObject.SetActive(false);
        }
    }

    public void ShowToast(string message)
    {
        if (toastCoroutine != null)
        {
            StopCoroutine(toastCoroutine);
        }
        toastCoroutine = StartCoroutine(ShowToastCoroutine(message));
    }

    private IEnumerator ShowToastCoroutine(string message)
    {
        toastText.text = message;
        toastText.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        toastText.gameObject.SetActive(false);
    }
}