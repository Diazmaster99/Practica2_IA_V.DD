﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {   

        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
        public BoardManager BoardManager { get; set; }                      //Store a reference to our Board which will set up the level.
        public int seed=2020;
        public bool ForPlanner = false;
        public int numEnemies;
        private CharacterBehaviour characterBehaviour;

        public List<GameObject> ActiveEnemies ;

        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

            //Get a component reference to the attached BoardManager script
            this.BoardManager = GetComponent<BoardManager>();

            characterBehaviour = GameObject.Find("Character").GetComponent<CharacterBehaviour>();
            characterBehaviour.BoardManager= BoardManager;
            
        }

        public void Start()
        {
            characterBehaviour.SetCurrentTarget(BoardManager.boardInfo.Exit);
            
        }

        //Initializes the game for each level.
        public void InitGame()
        {
            ActiveEnemies = new List<GameObject>(numEnemies);
            //Call the SetupScene function of the BoardManager script, pass it current level number.
            BoardManager.SetupScene(this.seed, this.ForPlanner,numEnemies);
            BoardManager.GenerateMap();
        }
    }
}
