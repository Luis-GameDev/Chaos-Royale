using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;

public class Player : NetworkBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject cameraHolder;
    private Camera cam;
    private GameObject ui;
    private GameObject camHold;


    // disable player script if not owner, so only the owner can control the player and other player prefabs dont react to input
    public override void OnStartClient()
    {
        base.OnStartClient();
        if(!base.IsOwner) {
            gameObject.GetComponent<Player>().enabled = false;
        }
        else {
            EnsureDependencies();
        }
    }

    void Start() {
        //UpdateCameras();
    }

    void Update()
    {
        if(base.IsOwner) {
           HandleInput(); 
        }
    }

    // ensures an instance of the player UI and camera holder are created for the player and that all references are properly set
    void EnsureDependencies() {
        if(!character) {
            character = gameObject.GetComponent<Character>();
        }
        gameObject.transform.Find("HP Bar").gameObject.SetActive(false);

        if(!ui) ui = Instantiate(playerUI, transform.position, Quaternion.identity);
        ui.GetComponent<UpdateCooldowns>().character = character;
        if(!camHold) camHold = Instantiate(cameraHolder, transform.position, Quaternion.identity);
        camHold.GetComponent<FixCam>().cameraTransform = this.transform;
        cam = camHold.GetComponentInChildren<Camera>();
        gameObject.GetComponent<PlayerHighlight>().playerCamera = cam;
        character.HPbar = camHold.GetComponentsInChildren<Image>().FirstOrDefault(img => img.gameObject.name == "HP");
    }

    void UpdateCameras() {
        if(!character) {
            character = gameObject.GetComponent<Character>();
        }
        gameObject.transform.Find("HP Bar").gameObject.SetActive(false);

        if(!ui) ui = Instantiate(playerUI, transform.position, Quaternion.identity);
        ui.GetComponent<UpdateCooldowns>().character = character;
        if(!camHold) {
            camHold = Instantiate(cameraHolder, transform.position, Quaternion.identity);
            camHold.GetComponent<FixCam>().cameraTransform = this.transform;
        }
        cam = camHold.GetComponentInChildren<Camera>();
        gameObject.GetComponent<PlayerHighlight>().playerCamera = cam;
        character.HPbar = camHold.GetComponentsInChildren<Image>().FirstOrDefault(img => img.gameObject.name == "HP");
        // set the camera for all canvas face camera scripts
        CanvasFaceCamera[] canvasFaceCameras = FindObjectsOfType<CanvasFaceCamera>();
        foreach (CanvasFaceCamera canvasFaceCamera in canvasFaceCameras)
        {
            cam = camHold.GetComponentInChildren<Camera>();
            //canvasFaceCamera.SetCamera(cam);
        }
    }

    void OnDestroy()
    {
        Destroy(ui);
        Destroy(camHold);
    }

    public void HandleInput() {
        
        if(!character || !cam) {
            return;
        }

        if (Input.GetMouseButton(1)) // 1 is RMB
        {
            Vector3 cursorWorldPosition = GetCursorWorldPosition();

            if(cursorWorldPosition != Vector3.zero) {
                character.Move(cursorWorldPosition);
            }
        }

        if(Input.GetKeyDown(KeyCode.Q) && character.globalCooldownLeft <= 0) {
            character.UseAbility(0);
            character.combatTimeLeft = character.combatTime;
        }
        if(Input.GetKeyDown(KeyCode.W) && character.globalCooldownLeft <= 0) {
            character.UseAbility(1);
            character.combatTimeLeft = character.combatTime;
        }
        if(Input.GetKeyDown(KeyCode.E) && character.globalCooldownLeft <= 0) {
            character.UseAbility(2);
            character.combatTimeLeft = character.combatTime;
        }
        if(Input.GetKeyDown(KeyCode.R) && character.globalCooldownLeft <= 0) {
            character.UseAbility(3);
            character.combatTimeLeft = character.combatTime;
        }
    }

    public Vector3 GetCursorWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                return hit.point;
            }
        }
        return Vector3.zero;
    }
}
