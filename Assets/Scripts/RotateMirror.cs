using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateMirror : MonoBehaviour
{
    public LayerMask mirrorLayer;
    private bool isClicked;
    private float closestOne = 100000;
    private GameObject closestMirror;
    float power;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hits;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if ((hits = Physics.RaycastAll(ray,float.MaxValue, mirrorLayer)) != null)
            {
                foreach(RaycastHit hit in hits)
                {
                    if(Vector3.Distance(hit.point, Camera.main.transform.position) < closestOne)
                    {
                        closestOne = Vector3.Distance(hit.point, Camera.main.transform.position);
                        closestMirror = hit.collider.gameObject;
                    }
                }
                if (closestMirror == gameObject)
                {
                    isClicked = true;
                    GameController.Instance.isGameStarted = true;
                }
            }
            else
            {
                isClicked = false;
                closestMirror = null;
                closestOne = float.MaxValue;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isClicked = false;
            closestMirror = null;
            closestOne = float.MaxValue;
        }
    }
    private void FixedUpdate()
    {
        if (isClicked)
        {
            transform.Rotate(Vector3.up, 1f);
        }
    }
}
