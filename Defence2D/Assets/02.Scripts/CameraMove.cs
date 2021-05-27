using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //카메라 이동
    Vector2 prevPos = Vector2.zero;
    float MoveSpeed = 10f;

    //카메라 이동범위 제한
    public Vector2 Center;
    public Vector2 Size;
    float Height;
    float Width;

    // Start is called before the first frame update
    void Start()
    {
        Height = Camera.main.orthographicSize;
        Width = Height * Screen.width / Screen.height;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Center, Size);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        prevPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 dir = (eventData.position - prevPos).normalized;
        Camera.main.transform.position -= dir * MoveSpeed * Time.deltaTime;
        prevPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        prevPos = Vector2.zero;
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void LateUpdate()
    {
        //카메라 이동범위 제한
        float lx = Size.x * 0.5f - Width;
        float clampX = Mathf.Clamp(Camera.main.transform.position.x, -lx + Center.x, lx + Center.x);

        float ly = Size.y * 0.5f - Height;
        float clampY = Mathf.Clamp(Camera.main.transform.position.y, -ly + Center.y, ly + Center.y);

        Camera.main.transform.position = new Vector3(clampX, clampY, -10f);
    }
   
}
