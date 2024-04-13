using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float speed;

    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer sprite;

    private Vector3Int lastPlayerPos;
    private Pathfinding pathfinder;
    private List<Node> waypoints;

    private bool canTurn = true;
    private bool canMove = true;

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

        lastPlayerPos = TilemapManager.instance.playerPos;
        pathfinder = new Pathfinding();

        Vector2Int eCell = (Vector2Int)TilemapManager.instance.WorldToCell(transform.position);
        Vector2Int pCell = (Vector2Int)TilemapManager.instance.playerPos;
        waypoints = pathfinder.getPathAStar(eCell, pCell);
    }

    private void OnEnable()
    {
        canMove = true;
        canTurn = true;
    }
    private void Update()
    {

        if (!canMove)
            return;

        if(lastPlayerPos != TilemapManager.instance.playerPos)
        {
            Vector2Int eCell = (Vector2Int)TilemapManager.instance.WorldToCell(transform.position);
            Vector2Int pCell = (Vector2Int)TilemapManager.instance.playerPos;
            waypoints = pathfinder.getPathAStar(eCell, pCell);
        }
            

        anim.SetBool("isEnemyWalking", body.velocity != Vector2.zero);

        flipSprite();
    }

    private void FixedUpdate()
    {
        if(waypoints != null && waypoints.Count > 0)
            Move(waypoints[0]);
    }

    private void Move(Node targetWaypoint)
    {

        if (body.bodyType == RigidbodyType2D.Static)
            return;


        Vector2 targetPos = TilemapManager.instance.CellToWorld((Vector2Int)TilemapManager.instance.playerPos);
        Vector2 velocity = (targetPos - (Vector2)transform.position).normalized * speed ;

        //velocity.x = HorizontalInput * Time.fixedDeltaTime;
        //velocity.y = VerticalInput * Time.fixedDeltaTime;

        velocity.Normalize();
        velocity *= speed;

        body.velocity = velocity;

        if((Vector2Int)TilemapManager.instance.WorldToCell(transform.position) == targetWaypoint.toVector2i())
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
