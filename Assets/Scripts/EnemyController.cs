using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField][Range(0f, 5f)] float speed = 1f;

    List<Node> path = new List<Node>();

    Enemy enemy;

    PathFinder pathFinder;
    GridManager gridManager;


    void Awake()
    {
        enemy = GetComponent<Enemy>(); 
        pathFinder = FindFirstObjectByType<PathFinder>();
        gridManager = FindFirstObjectByType<GridManager>();
    }
    void OnEnable()
    {
        RecalcualtePath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FollowPath()
    {
        for(int i = 0; i <path.Count; i++)
        {
            //Debug.Log(waypoint.name);
            Vector3 startPos = transform.position;
            Vector3 endPos = gridManager.GetPositionFromCoordinates(path[i].coordinates);

            if (endPos.y > startPos.y)
            {
                Vector3 verticaleEndPos = new Vector3(startPos.x + ((endPos.x - startPos.x) / 2), endPos.y, startPos.z + ((endPos.z - startPos.z) / 2));
                
            }
            else if (endPos.y < startPos.y)
            {
                Vector3 horisontalEndPos = new Vector3 (startPos.x + ((endPos.x - startPos.x) /2), startPos.y, startPos.z + ((startPos.z - startPos.z) /2));
            }

                //smooth movemnt between points
                float travelPercent = 0f;

            transform.LookAt(endPos);

            while(travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPos, endPos, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
        FinishPath();
    }

    void RecalcualtePath()
    {
        path.Clear();

        path = pathFinder.GetNewPath();
    }

    void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
    }

    void FinishPath()
    {
        gameObject.SetActive(false);
        enemy.penalizeGold();
    }
}
