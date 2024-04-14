using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float speed;

    [Header("Pathfinding")]
    [SerializeField] private LayerMask layerMask;

    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer sprite;

    private Pathfinding pathfinder;
    private List<Node> waypoints;

    private bool canTurn = true;
    private bool canMove = true;


    private GameObject target;
    public void setTarget(GameObject target)
    {
        this.target = target;
        pathfindToTarget();
    }

    private void pathfindToTarget()
    {
        if (!target)
            return;
 
        Vector2Int eCell = (Vector2Int)TilemapManager.instance.WorldToCell(transform.position);
        Vector2Int tCell = (Vector2Int)TilemapManager.instance.WorldToCell(target.transform.position);
        waypoints = pathfinder.getPathAStar(eCell, tCell);

       
    }

    private void Awake()
    {
        pathfinder = new Pathfinding(layerMask);
        target = null;
    }

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        GetComponent<Health>().onDie += () =>
        {
            canTurn = false;
            anim.SetBool("isEnemyWalking", false);
        };

 

    }

    private void OnEnable()
    {
        canMove = true;
        canTurn = true;
    }
    private void Update()
    {

       pathfindToTarget();
        
        for (int i = 1; i < waypoints.Count; i++)
            Debug.DrawLine(
               (Vector3)TilemapManager.instance.CellToWorld(waypoints[i - 1].toVector2i()),
               (Vector3)TilemapManager.instance.CellToWorld(waypoints[i].toVector2i()),
               Color.blue
           );


        anim.SetBool("isEnemyWalking", body.velocity != Vector2.zero);

        flipSprite();
    }

    private void FixedUpdate()
    {
        if (!canMove || !target)
            return;

        if (waypoints != null && waypoints.Count > 0)
            Move(waypoints[0]);
    }



    private void Move(Node targetWaypoint)
    {

        if (body.bodyType == RigidbodyType2D.Static)
            return;


        Vector2 waypointPos = TilemapManager.instance.CellToWorld(targetWaypoint.toVector2i());
        Vector2 velocity = (waypointPos - (Vector2)transform.position).normalized * speed ;

        Debug.DrawRay(transform.position, (waypointPos - (Vector2)transform.position).normalized, Color.yellow);

        velocity.Normalize();
        velocity *= speed;

        body.velocity = velocity;

        if(TilemapManager.instance.CellToWorld(targetWaypoint.toVector2i()) == (Vector2)transform.position)
        {
            waypoints.Remove(targetWaypoint);
        }
    }

    private void flipSprite()
    {

        if (!canTurn)
            return;

        float unsignedX = Mathf.Abs(transform.localScale.x);
        if (body.velocity.x < 0f)
            transform.localScale = new Vector2(-1 * unsignedX, transform.localScale.y);
        else if (body.velocity.x > 0f)
            transform.localScale = new Vector2(unsignedX, transform.localScale.y);
    }

    public void setTurning(bool canTurn)
    {
        this.canTurn = canTurn;
    }
}
