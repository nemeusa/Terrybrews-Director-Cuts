using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public float viewRadius;
    public float viewAngle;
    //EnemyCat _enemyCat;
    public bool IsFacingRight = true;
    [SerializeField] LayerMask _ignoreLayer;


    public bool InFOV(Transform target)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (target.position - transform.position).normalized, viewRadius, _ignoreLayer);
        if (hit.collider != null)
        {
            //Debug.Log("toco pared");

            // Hay una pared en el medio, asi que no sigue al jugador
            return false;
        }


        var dir = target.position - transform.position;

        if (dir.magnitude <= viewRadius)
        {

            Vector2 facingDir = IsFacingRight ? Vector2.right : Vector2.left;

            if (Vector2.Angle(facingDir, dir) <= viewAngle * 0.5f)
            {
                return PathManager.instance.InLineOfSight(transform.position, target.position);
            }
        }

        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector2 facingDir = IsFacingRight ? Vector2.right : Vector2.left;

        // Rotar límites según flip
        Vector3 leftBoundary = DirFromAngle(-viewAngle / 2, facingDir);
        Vector3 rightBoundary = DirFromAngle(viewAngle / 2, facingDir);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);
    }

    Vector3 DirFromAngle(float angleDegrees, Vector2 baseDir)
    {
        float rad = angleDegrees * Mathf.Deg2Rad;

        // Rota el vector baseDir
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector3(
            baseDir.x * cos - baseDir.y * sin,
            baseDir.x * sin + baseDir.y * cos,
            0
        );
    }
}
