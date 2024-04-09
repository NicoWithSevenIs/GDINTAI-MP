using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

public class NodeScript : MonoBehaviour
{

    private CinemachineVirtualCamera cam;
    [SerializeField] private LayerMask l;


    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


     

     
       
        
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit2D r = Physics2D.Raycast(mousePos, Vector2.zero, 100, l);

            if (r && r.collider.gameObject == gameObject)
            {
                PathfindingTester.instance.changeTarget(TilemapManager.instance.WorldToCell(transform.position));
                print(PowerUpManager.instance.getNameAt((Vector2Int)TilemapManager.instance.WorldToCell(transform.position)));
            }
                

        }
    }
}
