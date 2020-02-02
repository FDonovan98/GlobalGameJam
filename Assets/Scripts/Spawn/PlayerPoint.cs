
using UnityEngine;

public class PlayerPoint : MonoBehaviour
{ 
    void OnDrawGizmos()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }
}
