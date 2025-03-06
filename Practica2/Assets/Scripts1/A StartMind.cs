using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStartMind : AbstractPathMind
{
    public CharacterBehaviour characterBehaviour;
    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        /*if (BoardManager == null) return;
        if (LocomotionController.MoveNeed)
        {

            var boardClone = (BoardInfo)BoardManager.boardInfo.Clone();
            LocomotionController.SetNewDirection(PathController.GetNextMove(boardClone, LocomotionController.CurrentEndPosition(), new[] { this.currentTarget }));
        }
        return Locomotion.MoveDirection.Right;*/
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
