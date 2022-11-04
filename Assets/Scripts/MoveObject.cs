using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveObject : MonoBehaviour
{
    [SerializeField] private Transform rayPosForward;
    [SerializeField] private Transform rayPosBackward;
    [SerializeField] private MeshCollider _meshCollider;
    [SerializeField] private float Speed;
    public bool isObjectPlacedVertical = true;
    public bool isObjectReverse;
    public List<Transform> gapHits;
    public Transform rayPos;
    private float gapSize;


    private void Awake()
    {
        gapHits = new List<Transform>();
        if (transform.forward == Vector3.right || transform.forward == Vector3.left)
            isObjectPlacedVertical = false;
        if (transform.forward == Vector3.back || transform.forward == Vector3.left)
            isObjectPlacedVertical = true;
    }

    public void Move(bool isVertical, bool isPositive)
    {
        if (isVertical != isObjectPlacedVertical)
            return;
        VarSet(isPositive);
        CheckEmpty(rayPos);
        GridReEnable();
    }
    private void VarSet(bool isPositive)
    {
        if (isPositive == true && isObjectReverse == false)
        {
            rayPos = rayPosForward;
            gapSize = 2f;
        }
        else if (isPositive == true && isObjectReverse == true)
        {
            rayPos = rayPosBackward;
            gapSize = -2f;
        }
        else if (isPositive == false && isObjectReverse == true)
        {
            rayPos = rayPosForward;
            gapSize = 2f;
        }
        else if (isPositive == false && isObjectReverse == false)
        {
            rayPos = rayPosBackward;
            gapSize = -2f;
        }

    }

    private void Update()
    {
        Debug.DrawLine(rayPosBackward.transform.position, rayPosBackward.transform.forward * 100f, Color.blue);
        Debug.DrawLine(rayPosForward.transform.position, rayPosForward.transform.forward * 100f, Color.red);
    }
    private void CheckEmpty(Transform rayPos)
    {
        RaycastHit hit;

        if (Physics.Raycast(rayPos.transform.position, rayPos.transform.forward, out hit, 100.0f))
        {
            Debug.Log("Move Method");

            if (hit.transform.CompareTag("EmptyGrid"))
            {
                Debug.Log("EmptyGrid");
                gapHits.Add(hit.transform);
                hit.transform.GetComponent<BoxCollider>().enabled = false;
                CheckEmpty(rayPos);
            }
            else if (hit.transform.CompareTag("ExitWay"))
            {
                Debug.Log("ExitWay");
                _meshCollider.enabled = false;
                transform.DOMove(transform.position + transform.forward * gapSize * gapHits.Count, gapHits.Count * (1 / Speed) * 1.5f)
                    .OnComplete(() => Exit(hit));

            }
            else
            {
                Debug.Log("Move");
                transform.DOMove(transform.position + transform.forward * gapSize * gapHits.Count, gapHits.Count * (1 / Speed) * 1.5f);
                return;
            }
        }

    }
    private void GridReEnable()
    {
        for (int i = 0; i < gapHits.Count;)
        {
            gapHits[i].GetComponent<BoxCollider>().enabled = true;
            gapHits.Remove(gapHits[i]);
        }
    }
    private void Exit(RaycastHit hit)
    {
        transform.DOMove(hit.transform.position, (1 / Speed) * 1.5f).OnComplete(() =>
           {
               this.gameObject.SetActive(false);
               if (this.gameObject.CompareTag("Player"))
               {
                   InputManager.instance._levelCompleted.SetActive(true);
               }
           });

    }
}
