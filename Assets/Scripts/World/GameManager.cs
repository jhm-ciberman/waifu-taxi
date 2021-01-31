using UnityEngine;

namespace WaifuTaxi
{
    public class GameManager : MonoBehaviour
    {
        public WorldComponent worldComponent;

        public CameraController cameraController;

        private RoutePlanner _planner;

        public DialogueManager dialogueManager = null;

        public void Start()
        {
            var world = new World(new Vector2Int(20, 20));

            var player = this.worldComponent.GenerateWorld(world);

            this.cameraController.SetTarget(player.transform);

            this._planner = new RoutePlanner(world, player);

            if (this.dialogueManager != null) { // With UI
                this._planner.onIndication += (e) => {
                    if(e.pathWasRestarted) {
                        dialogueManager.failDialogue(e.indication, e.prevIndication);
                    } else {
                        dialogueManager.GiveIndication(e.indication);
                    }
                };
                this._planner.onPathFinished += this.dialogueManager.nextPasajero;
                //player.onCollision += this.dialogueManager.failDialogue;
            } else { // Without ui
                this._planner.onIndication += (e) => {
                    Debug.Log("Indication: " + e.indication);
                    Debug.Log("PrevIndication: " + e.prevIndication);
                    Debug.Log("Restarted: " + e.pathWasRestarted); 
                };
                this._planner.onPathFinished += () => {
                    Debug.Log("Path finished");
                };
            }
            this._planner.UpdatePath();
        }

        void Update()
        {
            this._planner.UpdatePath();
        }
    }
}