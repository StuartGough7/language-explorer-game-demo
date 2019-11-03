using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoop : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prevCeiling;
    public GameObject prevFloor;
    public GameObject ceiling;
    public GameObject floor;
    public GameObject player;
    public int sizeOfFloor = 20;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x > floor.transform.position.x)
        {
            var tempCeilng = prevCeiling;
            var tempFloor = prevFloor;
            prevCeiling = ceiling;
            prevFloor = floor;
            prevCeiling.transform.position += new Vector3(sizeOfFloor, 0, 0);
            prevFloor.transform.position += new Vector3(sizeOfFloor, 0, 0);
            ceiling = tempCeilng;
            floor = tempFloor;
        }
        
    }
}
