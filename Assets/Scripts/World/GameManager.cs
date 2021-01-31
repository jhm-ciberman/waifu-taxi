using UnityEngine;

namespace WaifuTaxi
{
    public class GameManager : MonoBehaviour
    {
        public WorldComponent worldComponent;

        private RoutePlanner _planner;

        public DialogueManager dialogueManager = null;

        public void Start()
        {
            var world = new World(new Vector2Int(20, 20));

            var player = this.worldComponent.GenerateWorld(world);

            this._planner = new RoutePlanner(world, player);

            if (this.dialogueManager != null) { // With UI
                this._planner.onIndication += (e) => {
                    Debug.Log("Indication: " + e.indication);
                    dialogueManager.GiveIndication(e.indication);
                    Debug.Log("PrevIndication: " + e.prevIndication);
                    if(e.pathWasRestarted)
                    {
                        dialogueManager.failDialogue();
                    }
                    Debug.Log("Restarted: " + e.pathWasRestarted);
                };
            } else { // Without ui
                this._planner.onIndication += (e) => {
                    Debug.Log("Indication: " + e.indication);
                    Debug.Log("PrevIndication: " + e.prevIndication);
                    Debug.Log("Restarted: " + e.pathWasRestarted); 
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