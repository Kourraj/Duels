using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// TODO - COMMENT!!!!
public class PageManager : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private Vector3 panelLocation;
    public float percentThreshold = 0.2f;
    public float easing = 0.5f;
    public int totalPages = 2;
    [SerializeField]
    private int currentPage = 2;

    public RectTransform canvas;

    // Start is called before the first frame update
    void Start(){
        panelLocation = transform.position;
    }
    
    public void OnDrag(PointerEventData data){
        // The amount we've moved
        float difference = (data.pressPosition.x - data.position.x);
        // Move the current panel
        transform.position = panelLocation - new Vector3(difference / 5, 0, 0);
    }

    public void OnEndDrag(PointerEventData data){
        // Percentage of the screen we've swiped
        float width = canvas.rect.width;
        Debug.Log(width);
        float percentage = (data.pressPosition.x - data.position.x) / (width);

        // Check if we've moved enough
        if(Mathf.Abs(percentage) >= percentThreshold)
        {
            // Create a new location, and set it to the idle state.
            Vector3 newLocation = panelLocation;

            // If we've moved to the right
            if(percentage > 0)
            {
                newLocation += new Vector3(-width, 0, 0);
            }
            else if (percentage <0) 
            {
                newLocation += new Vector3(width, 0, 0);   
            }
            transform.position = newLocation;
            panelLocation = newLocation;
        }
        else
        {
            transform.position = panelLocation;
        }
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds){
        float t = 0f;
        while(t <= 1.0){
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}