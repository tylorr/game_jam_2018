using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform spawnLocation;
    public RectTransform boundingRectTransform;

    public Rect GetBoundingRect()
    {
        var corners = new Vector3[4];
        boundingRectTransform.GetWorldCorners(corners);
        var topLeft = corners[0];
        return new Rect(topLeft, boundingRectTransform.rect.size);
    }
}
