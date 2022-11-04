using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public GameObject _levelCompleted;

    private Vector2 mouseFirstPos;
    private Vector2 mouseVector;

    private bool isVertical;
    private bool isPositive;

    private GameObject selectedObject;

    private void Awake() {
        instance=this;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray,out hit, 100f))
            {
                if(hit.transform.CompareTag("Selectible") || hit.transform.CompareTag("Player"))
                {
                    selectedObject=hit.transform.gameObject;
                }
            }
            mouseFirstPos=Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            mouseVector.x=Input.mousePosition.x-mouseFirstPos.x;
            mouseVector.y = Input.mousePosition.y - mouseFirstPos.y;
            CalculateDirection(mouseVector.x, mouseVector.y);
            MoveSelectedObject();
        }
        
    }
    private void CalculateDirection(float x, float y)
    {
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            isVertical = false;
            if (x < 0)
                isPositive = false;
            else
                isPositive = true;
        }
        else
        {
            isVertical = true;
            if (y < 0)
                isPositive = false;
            else
                isPositive = true;
        }
    }
    private void MoveSelectedObject()
    {
        Debug.Log(isVertical);
        if(selectedObject!=null)
            selectedObject.GetComponent<MoveObject>().Move(isVertical,isPositive);
        selectedObject=null;
    }
}
