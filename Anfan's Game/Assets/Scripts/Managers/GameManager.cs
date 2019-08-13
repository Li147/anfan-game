using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// This class is responsible for selecting a player's target
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private Enemy currentTarget;

    private Camera mainCamera;

    private static GameManager instance;
    private int targetIndex;

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

    public Camera MyCamera { get => mainCamera; set => mainCamera = value; }

    private void Start()
    {
        MyCamera = Camera.main;
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

            RaycastHit2D hit = Physics2D.Raycast(MyCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null)
            {
                IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();

                // if our mouse clicks on an enemy...
                if (hit.collider != null && (hit.collider.tag == "enemy" || hit.collider.tag == "interactable") && player.MyInteractables.Contains(entity))
                {
                    entity.Interact();
                }
            }
        

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


}
