using System.Collections;
using UnityEngine;

public class WayPointMover : MonoBehaviour
{
    public Transform wayPointParent;
    public float moveSpeed = 2f;
    public float waitTime = 2f;
    public bool loopWaypoints = true;

    private Transform[] waypoints;
    private int currentWaypointIndex;
    private bool isWaiting;
    private Animator animator;

    private float lastInputX;
    private float lastInputY;


    void Start()
    {
        animator = GetComponent<Animator>();
        waypoints = new Transform[wayPointParent.childCount];

        for(int i = 0; i< wayPointParent.childCount; i++)
        {
            waypoints[i] = wayPointParent.GetChild(i);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(PauseController.IsGamePaused || isWaiting)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", lastInputX);
            animator.SetFloat("LastInputY", lastInputY);
            return;
        }
        MoveToWayPoint();
    }
    void MoveToWayPoint()
    {
        Transform target = waypoints[currentWaypointIndex];
        Vector2 direction = (target.position - transform.position).normalized;

        if(direction.magnitude > 0f)
        {
            lastInputX = direction.x;
            lastInputY = direction.y;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime); 

        animator.SetFloat("InputX",direction.x);
        animator.SetFloat("InputY", direction.y);
        animator.SetBool("isWalking", direction.magnitude > 0f);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAtWayPoint());
        }
    }
    IEnumerator WaitAtWayPoint()
    {
        isWaiting = true;
        animator.SetBool("isWalking", false);

        animator.SetFloat("LastInputX", lastInputX);
        animator.SetFloat("LastInputY", lastInputY);
        yield return new WaitForSeconds(waitTime);

        currentWaypointIndex = loopWaypoints ? (currentWaypointIndex + 1) % waypoints.Length : Mathf.Min(currentWaypointIndex + 1, waypoints.Length - 1);
        isWaiting = false;

    }
}
