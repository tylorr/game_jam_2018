using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Camera))]
public class DeadzoneCamera : MonoBehaviour 
{
    public Renderer target;
    public Rect deadzone;

    public List<Rect> limits = new List<Rect>();

    protected Camera _camera;
    protected Vector3 _currentVelocity;

    public void Start()
    {
        _currentVelocity = Vector3.zero;

        _camera = GetComponent<Camera>();
        if(!_camera.orthographic)
        {
            Debug.LogError("deadzone script require an orthographic camera!");
            Destroy(this);
        }
    }

    public void LateUpdate()
    {
        float localX = target.transform.position.x - transform.position.x;
        float localY = target.transform.position.y - transform.position.y;

        var nextPosition = transform.position;

        if (localX < deadzone.xMin)
        {
            nextPosition.x += localX - deadzone.xMin;
        }
        else if (localX > deadzone.xMax)
        {
            nextPosition.x += localX - deadzone.xMax;
        }

        if (localY < deadzone.yMin)
        {
            nextPosition.y += localY - deadzone.yMin;
        }
        else if (localY > deadzone.yMax)
        {
            nextPosition.y += localY - deadzone.yMax;
        }

        Rect camWorldRect = new Rect();
        camWorldRect.min = new Vector2(nextPosition.x - _camera.aspect * _camera.orthographicSize, nextPosition.y - _camera.orthographicSize);
        camWorldRect.max = new Vector2(nextPosition.x + _camera.aspect * _camera.orthographicSize, nextPosition.y + _camera.orthographicSize);

        bool foundLimits = false;
        for (int i = 0; i < limits.Count; ++i)
        {
            if (limits[i].Contains(target.transform.position))
            {
                Vector3 localOffsetMin = limits[i].min + camWorldRect.size * 0.5f;
                Vector3 localOffsetMax = limits[i].max - camWorldRect.size * 0.5f;

                localOffsetMin.z = localOffsetMax.z = nextPosition.z;

                nextPosition = Vector3.Max(nextPosition, localOffsetMin);
                nextPosition = Vector3.Min(nextPosition, localOffsetMax);
                foundLimits = true;
                break;
            }
        }

        if (foundLimits)
        {
            transform.position = nextPosition;
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(DeadzoneCamera))]
public class DeadZonEditor : Editor
{
    public void OnSceneGUI()
    {
        DeadzoneCamera cam = target as DeadzoneCamera;

        Vector3[] vert = 
        {
            cam.transform.position + new Vector3(cam.deadzone.xMin, cam.deadzone.yMin, 0),
            cam.transform.position + new Vector3(cam.deadzone.xMax, cam.deadzone.yMin, 0),
            cam.transform.position + new Vector3(cam.deadzone.xMax, cam.deadzone.yMax, 0),
            cam.transform.position + new Vector3(cam.deadzone.xMin, cam.deadzone.yMax, 0)
        };

        Color transp = new Color(0, 0, 0, 0);
        Handles.DrawSolidRectangleWithOutline(vert, transp, Color.red);

        for(int i = 0; i < cam.limits.Count; ++i)
        {
            Vector3[] vertLimit =
           {
                new Vector3(cam.limits[i].xMin, cam.limits[i].yMin, 0),
                new Vector3(cam.limits[i].xMax, cam.limits[i].yMin, 0),
                new Vector3(cam.limits[i].xMax, cam.limits[i].yMax, 0),
                new Vector3(cam.limits[i].xMin, cam.limits[i].yMax, 0)
            };

            Handles.DrawSolidRectangleWithOutline(vertLimit, transp, Color.green);
        }
    }
}
#endif