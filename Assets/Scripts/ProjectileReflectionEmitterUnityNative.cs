using System;
using System.Collections;
using UnityEditor;
using UnityEngine;


public class ProjectileReflectionEmitterUnityNative : MonoBehaviour
{
    [SerializeField] private int maxReflectionCount = 5;
    [SerializeField] private float maxStepDistance = 200;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int lineLength = 100;
    public LayerMask layerMask;
    private int hitCount;
    public ParticleSystem[] ps;
    public GameObject endPortal;

    private void Start()
    {
        StartCoroutine(Raytrace());
    }

    private IEnumerator Raytrace()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.025f);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward);
            lineRenderer.positionCount = 2;

            hitCount = 0;
            if (lineRenderer.positionCount < lineLength)
            {
                DrawPredictedReflectionPattern(transform.position + transform.forward * 0.75f, transform.forward, maxReflectionCount);
            }
        }
    }

    private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {
        if (reflectionsRemaining == 0)
        {
            return;
        }

        Vector3 startingPosition = position;

        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        bool hitObstacle = false;
        bool hitSameColorMirror = false;
        if (Physics.Raycast(ray, out hit, maxStepDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.tag == gameObject.tag)
            {
                hitSameColorMirror = true;
            }
            else
            {
                hitObstacle = true;
                if (hit.collider.tag == gameObject.tag + "End")
                {
                    endPortal.SetActive(true);
                    GameObject.FindGameObjectWithTag(gameObject.tag + "End").GetComponent<SphereCollider>().enabled = false;

                    GameObject[] objs = GameObject.FindGameObjectsWithTag(gameObject.tag);
                    foreach (GameObject obj in objs)
                    {
                        RotateMirror rm = obj.GetComponent<RotateMirror>();
                        if(rm != null)
                        {
                            Destroy(rm);
                            GameController.Instance.remainingMirrors--;
                            if(GameController.Instance.remainingMirrors == 0)
                            {
                                GameController.Instance.EndLevel();
                            }
                        }
                    }
                    return;
                }
            }
            direction = Vector3.Reflect(direction, hit.normal);
            position = hit.point;
            ps[hitCount].gameObject.transform.position = position;
            ps[hitCount].gameObject.SetActive(true);
            hitCount++;
        }
        else
        {
            hitSameColorMirror = true;
            if (ps[hitCount].gameObject.activeSelf)
            {
                ps[hitCount].gameObject.SetActive(false);

            }
            position += direction * maxStepDistance;
        }
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
        
        if (hitObstacle)
        {
            return;
        }

        DrawPredictedReflectionPattern(position, direction, reflectionsRemaining - 1);
    }
}
