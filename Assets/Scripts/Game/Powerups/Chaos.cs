using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaos : MonoBehaviour
{
    private List<Vector3Int> visited;

    private void Start()
    {
        visited = new List<Vector3Int>();
        ResetVisited();
    }

    private Vector3Int toVec3Int(Vector3 vec)
    {
        return new Vector3Int((int)vec.x, (int)vec.y, (int)vec.z);
    }

    private void ResetVisited()
    {
        visited.Clear();
        List<GameObject> bases = Game.instance.playerBases;
        foreach (var b in bases)
        {
            Vector3Int v = toVec3Int(b.transform.position);
            print(v);
            visited.Add(v);
        }

        bases = Game.instance.enemyBases;
        foreach (var b in bases)
        {
            Vector3Int v = toVec3Int(b.transform.position);
            print(v);
            visited.Add(v);
        }

       

    }

    private void OnEnable()
    {
        if (visited != null)
            ResetVisited();
       
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Dictionary<string, List<GameObject>> r = new Dictionary<string, List<GameObject>>()
        {
            {"Player", Game.instance.playerBases},
            {"Enemy", Game.instance.enemyBases}
        };

        if (!r.TryGetValue(collision.tag, out List<GameObject> a))
            return;

        

    }
}
