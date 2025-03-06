using System.Collections.Generic;
using UnityEngine;

public class MovementByClick : MonoBehaviour
{
    private List<Vector2Int> path;
    private int speed = 5;
    private int x = 0;
    private bool isMoving;

    void Update()
    {
        /*if (Mouse.current.leftButton.wasPressedThisFrame && !isMoving)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if (hit && hit.collider.gameObject.TryGetComponent(out Walkable component))
            {
                path = PathGenerator.pathFinding.FindPath(PositionToVector2Int(), component.GetCellID());
                isMoving = true;
            }
        }*/
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Patrol(path);
        }
    }

    private void MoveCharacter(Vector2 destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.fixedDeltaTime);
    }

    private void Patrol(List<Vector2Int> patrolPoints)
    {
        float distance = Vector2.Distance(transform.position, patrolPoints[x]);
        if (distance > 0) MoveCharacter(patrolPoints[x]);
        else if (distance == 0) x++;
        if (x == patrolPoints.Count)
        {
            x = 0;
            isMoving = false;
        }
    }

    private Vector2Int PositionToVector2Int()
    {
        return Vector2Int.RoundToInt(transform.position);
    }
}
