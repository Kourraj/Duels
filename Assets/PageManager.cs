using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageManager : MonoBehaviour, IDragHandler, IEndDragHandler
{
    // The location of the current panel.
    private Vector3 panelLocation;
    // The amount of the screen you need to swipe to switch menus.
    public float percentThreshold = 0.2f;
    // The time to transition to the next screen.
    public float easing = 0.5f;
    // The total number of pages cycling through
    public int totalPages = 1;
    // The index of the page, index starts at 1.
    public int currentPage = 1;

    void Start()
    {
        // Sets the panelLocation to the current location
        panelLocation = transform.position;
    }

    // Used when the finger is present on the screen
    public void OnDrag(PointerEventData data)
    {
        // Get's the distance the finger moved.
        float difference = data.pressPosition.x - data.position.x;

        // Move the panel according to how much the finger moved.
        transform.position = panelLocation - new Vector3(difference, 0, 0);
    }

    // Used when the finger is lifted from the screen
    public void OnEndDrag(PointerEventData data)
    {
        // Get the % of the screen the finger moved across
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        // Check if we're above the % threshold so we can move to the next menu
        if(Mathf.Abs(percentage) >= percentThreshold)
        {
            // Set the new location to the current location
            // We use this if there's no next page in the target direction.
            Vector3 newLocation = panelLocation;

            // Check which way we moved, and that there's another page in that direction
            if(percentage > 0 && currentPage < totalPages)
            {
                // Increment the page number
                currentPage++;
                // Set the new location to the next page's location
                newLocation += new Vector3(-Screen.width, 0, 0);
            }
            else if(percentage < 0 && currentPage > 1)
            {
                currentPage--;
                newLocation += new Vector3(Screen.width, 0, 0);
            }

            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        // Start the animation at time = 0
        float t = 0f;
        // Loop through the full animation.
        while(t <= 1f)
        {
            // Increases t with time
            t += Time.deltaTime / seconds;

            // Get the progress through the lerp. We use a smoothstep to make it a more natural animation.
            float smoothStep = Mathf.SmoothStep(0f, 1f, t);

            // Actually move the panel.
            transform.position = Vector3.Lerp(startpos, endpos, smoothStep);

            // Wait one frame
            yield return null;
        }
    }
}