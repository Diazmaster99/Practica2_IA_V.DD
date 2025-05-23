﻿using Assets.Scripts.DataStructures;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Locomotion))]
    public class CharacterBehaviour : MonoBehaviour
    {

        protected Locomotion LocomotionController;
        protected AbstractPathMind PathController;
        public BoardManager BoardManager { get; set; }
        protected CellInfo currentTarget;
        public CellInfo characterPos;
        public CellInfo CharacterPosition() => this.LocomotionController.CurrentPosition();

        void Awake()
        {

            PathController = GetComponentInChildren<AbstractPathMind>();
            PathController.SetCharacter(this);
            LocomotionController = GetComponent<Locomotion>();
            LocomotionController.SetCharacter(this);

        }

        void Update()
        {
            if (BoardManager == null) return;
            if (LocomotionController.MoveNeed)
            {
                //characterPos = LocomotionController.CurrentPosition();
                var boardClone = (BoardInfo)BoardManager.boardInfo.Clone();
                LocomotionController.SetNewDirection(PathController.GetNextMove(boardClone, LocomotionController.CurrentEndPosition(), new[] { this.currentTarget }));

            }
        }



        public void SetCurrentTarget(CellInfo newTargetCell)
        {
            this.currentTarget = newTargetCell;
        }
    }
}

