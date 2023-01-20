using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraContainsMultiple : MonoBehaviour
{
    public List<Transform> targets;

    public float smoothTimeX = 0.5f;
    public float smoothTimeY = 0.5f;

    public float minZoom = 15f;
    public float maxZoom = 4f;
    private float zoomLimiter = 20f;

    private Vector3 offset = new Vector3(0f, 0f, -10f);

    private Vector3 velocity;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>(); 
    }

    void LateUpdate()
    {
        if (targets.Count == 0){
            if (GameManager.instance.selectedCharacter != null){
                // find the clone of the selected character
                GameObject clone = GameObject.Find(GameManager.instance.selectedCharacter.name + "(Clone)");
                if (clone != null){
                    targets.Add(clone.transform);
                }
            }
            else{
                return;
            }
        }

        

        Move();
        Zoom();
    }

    private void Move(){
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = new Vector3(Mathf.SmoothDamp(transform.position.x, newPosition.x, ref velocity.x, smoothTimeX), Mathf.SmoothDamp(transform.position.y, newPosition.y, ref velocity.y, smoothTimeY), transform.position.z);
    }

    private void Zoom(){
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    private float GetGreatestDistance(){
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }

    private Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }
}
