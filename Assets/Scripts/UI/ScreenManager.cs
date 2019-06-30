using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {

    public List<RectTransform> screens = new List<RectTransform>(); // List of avaialble screens

    [Range(0.0f, 1.0f)]
    public float scaleRate = 0.5f;

    private int currentScreenIndex;    // The currently active screen
    private List<Vector3> startingSizes = new List<Vector3>();

    // Use this for initialization
    void Start () {
        currentScreenIndex = -1;

        // Initialize the starting sizes
        foreach(var screen in screens)
        {
            startingSizes.Add(screen.localScale);
        }
	}
	
    // Change to a new scene
    public void ChangeScreens(int screenIndex)
    {
        // Index out of bounds
        if(screenIndex > screens.Count)
        {
            Debug.LogError("Change screens index out of bounds");
            return;
        }

        if (currentScreenIndex != -1)
        {
            hideScreen(currentScreenIndex);
        }

        showScreen(screenIndex);
    }

    // Show the screen
    private void showScreen(int screenToShow)
    {
        StartCoroutine(ShowCoroutine(screenToShow));
    }

    // Hide the screen
    private void hideScreen(int screenToHide)
    {
        StartCoroutine(HideCoroutine(screenToHide));
    }

    // Coroutine to do the scaling up
    private IEnumerator ShowCoroutine(int transformIndex)
    {
        Vector3 targetScale = startingSizes[transformIndex];
        RectTransform transformToScale = screens[transformIndex];
        transformToScale.localScale = Vector3.zero;

        transformToScale.gameObject.SetActive(true);

        // Scale up
        while(transformToScale.localScale.magnitude < targetScale.magnitude * 0.95)
        {
            transformToScale.localScale = Vector3.Lerp(transformToScale.localScale, targetScale, scaleRate);

            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
    }

    // Coroutine to do the scaling down
    private IEnumerator HideCoroutine(int transformIndex)
    {
        Vector3 targetScale = Vector3.zero;
        RectTransform transformToScale = screens[transformIndex];

        // Scale down
        while (transformToScale.localScale.normalized.magnitude > 0.05)
        {
            transformToScale.localScale = Vector3.Lerp(transformToScale.localScale, targetScale, scaleRate);

            yield return new WaitForSeconds(0.01f);
        }

        transformToScale.gameObject.SetActive(false);

        yield return null;
    }

}
