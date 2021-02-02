using UnityEngine;

namespace WaifuTaxi
{
    public class GameManager : MonoBehaviour
    {
        public WorldComponent worldComponent;

        public CameraController cameraController;

        private RoutePlanner _planner;

        public DialogueManager dialogueManager = null;

        public GUIManager guiManager = null;

        public void Start()
        {
            var world = new World(new City());

            var player = this.worldComponent.GenerateWorld(world);

            this.cameraController.SetTarget(player.transform);

            this._planner = new RoutePlanner(world, player);

            this.dialogueManager.changeCharacter += this.guiManager.ChangeCharacter;
            this.dialogueManager.changeSprite += this.guiManager.SetExpression;
            this.guiManager.ChangeCharacter(this.dialogueManager.character);

            AudioManager.Instance.PlaySound("music_intro", false, (s) => {
                AudioManager.Instance.PlayMusic("music_loop", true);
            });

            this._planner.onIndication += (e) => {
                if(e.pathWasRestarted) {
                    dialogueManager.FailDialogue(e.indication, e.prevIndication);
                } else {
                    dialogueManager.GiveIndication(e.indication);
                }
            };
            this._planner.onPathFinished += this.dialogueManager.NextCharacter;
            this._planner.UpdatePath();

            

        }

        void Update()
        {
            this._planner.UpdatePath();

            if (Input.GetKeyDown(KeyCode.Space)) {
                this.dialogueManager?.NextCharacter();
            }
        }
    }
}