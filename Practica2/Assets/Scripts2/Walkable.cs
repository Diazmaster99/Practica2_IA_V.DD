using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkable : MonoBehaviour
{
    private Vector2Int _cellID;
    private int _gCost;
    private int _hCost;
    private int _fCost;
    private Walkable _cameFromCell;

    void Start()
    {
        _cellID = Vector2Int.RoundToInt(transform.position);
    }

    public Vector2Int GetCellID()
    {
        return _cellID;
    }

    public void SetGCost(int value)
    {
        _gCost = value;
    }

    public int GetGCost()
    {
        return _gCost;
    }

    public void SetHCost(int value)
    {
        _hCost = value;
    }

    public void CalculateFCost()
    {
        _fCost = _gCost + _hCost;
    }

    public int GetFCost()
    {
        return _fCost;
    }

    public void SetCameFromCell(Walkable previousCell)
    {
        _cameFromCell = previousCell;
    }

    public Walkable GetCameFromCell()
    {
        return _cameFromCell;
    }
}
