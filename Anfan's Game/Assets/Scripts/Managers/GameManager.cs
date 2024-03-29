﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// This class is responsible for selecting a player's target
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Player player;

    [SerializeField]
    private LayerMask clickableLayer, groundLayer;

    private Enemy currentTarget;

    private Camera mainCamera;

    private int targetIndex;

    //GAME VALUES
    [SerializeField]
    private Timer timer;

    [SerializeField]
    private int hungerDrainRate;



    // keeps track of a set of all tiles we CANNOT WALK ON e.g. water tiles
    private HashSet<Vector3Int> blocked = new HashSet<Vector3Int>();

    
    public Camera MyCamera { get => mainCamera; set => mainCamera = value; }
    public HashSet<Vector3Int> Blocked { get => blocked; set => blocked = value; }

    private void Start()
    {
        MyCamera = Camera.main;
        InvokeRepeating("HungerDrain", 5.0f, 10.0f);
        InvokeRepeating("ManaRegen", 1.0f, 1.0f);
        InvokeRepeating("CheckHunger", 1.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();

        NextTarget();
    }

    // when player mouse clicks a target
    private void ClickTarget() {

        // deals with clicks using the LEFT MOUSE BUTTON
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {

            //DEBUGGING PURPOSES=============================================================
            //Vector3Int hello = Player.MyInstance.tileMap.WorldToCell(Input.mousePosition);
            //Debug.Log("Vector3Int: " + hello.ToString());
            //Vector3 sup = MyCamera.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log("Vector3: " + sup.ToString());

            RaycastHit2D hit = Physics2D.Raycast(MyCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
                        
            if (hit.collider != null && hit.collider.tag == "enemy") { // if my mouse clicks something

                DeSelectTarget();

                SelectTarget(hit.collider.GetComponent<Enemy>());
                
              
            } else { // if my mouse clicks nothing

                UIManager.MyInstance.HideTargetFrame();

                DeSelectTarget();

                currentTarget = null;
                player.MyTarget = null;
            }
        }
        // deals with clicks on the RIGHT MOUSE BUTTON
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()) {

            RaycastHit2D hit = Physics2D.Raycast(MyCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayer);

            if (hit.collider != null)
            {
                IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();

                // if our mouse clicks on an enemy...
                if (hit.collider != null && (hit.collider.tag == "enemy" || hit.collider.tag == "interactable") && player.MyInteractables.Contains(entity))
                {
                    entity.Interact();
                }
            }
            // A* star pathfinding code (buggy)
            //else
            //{
            //    hit = Physics2D.Raycast(MyCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, groundLayer);

            //    if (hit.collider != null)
            //    {

            //        player.GetPath(MyCamera.ScreenToWorldPoint(Input.mousePosition));

                   
            //    }
            //}
        

        }
        
    }

    private void NextTarget()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DeSelectTarget();

            if (Player.MyInstance.MyAttackers.Count > 0)
            {
                if (targetIndex < Player.MyInstance.MyAttackers.Count)
                {
                    SelectTarget(Player.MyInstance.MyAttackers[targetIndex]);
                    targetIndex++;
                    if (targetIndex >= Player.MyInstance.MyAttackers.Count)
                    {
                        targetIndex = 0;
                    }
                }
                else
                {
                    targetIndex = 0;
                }
               
            }
        }
    }

    private void SelectTarget(Enemy enemy)
    {
        currentTarget = enemy;
        player.MyTarget = currentTarget.Select();
        UIManager.MyInstance.ShowTargetFrame(currentTarget);
    }

    private void DeSelectTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.DeSelect();
        }
    }


    #region
    // drains player hunger
    private void HungerDrain()
    {
        player.MyHunger.MyCurrentValue -= hungerDrainRate;
    }

    //checks if player is too hungry
    private void CheckHunger()
    {
        if (player.MyHunger.MyCurrentValue <= 0)
        {
            player.TakeDamage(5, this.transform);
        }
    }

    // increases player mana
    private void ManaRegen()
    {
        float rate = player.MyManaRegenRate;
        player.MyMana.MyCurrentValue += rate;
    }

    #endregion

}
