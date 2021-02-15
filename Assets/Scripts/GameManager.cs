using UnityEngine;

namespace WaifuDriver
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private WorldGenerator _worldGenerator = null;

        [SerializeField] private CameraController _cameraController = null;

        [SerializeField] private DialogueManager _dialogueManager = null;

        [SerializeField] private GUIManager _guiManager = null;

        private RoutePlanner _planner;

        private int _livesTotal = 5;

        private int _livesCount = 5;


        public void Start()
        {
            var world = new World(new City());

            var pathfinder = new Pathfinder(world);
            var player = this._worldGenerator.GenerateWorld(world, pathfinder);
            this._planner = new RoutePlanner(pathfinder, player);

            this._cameraController.SetTarget(player.transform);


            this._dialogueManager.onCharacterChanged += this._guiManager.ChangeCharacter;
            this._dialogueManager.onEmotionChanged += this._guiManager.SetExpression;
            this._dialogueManager.onAnswerCorrect += this.AddLife;
            this._dialogueManager.onAnswerIncorrect += this.RemoveLife;

            this._guiManager.ChangeCharacter(this._dialogueManager.character);

            AudioManager.Instance.PlaySound("music_intro", false, (s) => {
                AudioManager.Instance.PlayMusic("music_loop", true);
            });

            this._planner.onIndication += this.OnIndication;
            this._planner.onPathFinished += this._dialogueManager.NextCharacter;
            this._planner.UpdatePath();

            this._guiManager.SetStarsCount(this._livesCount);
        }

        private void OnIndication(IndicationEvent e)
        {
            this._guiManager.SetDebugIndication(e.indication);
            if(e.pathWasRestarted) {
                this.OnRouteFail(e.indication, e.prevIndication);
            } else {
                this._dialogueManager.GiveIndication(e.indication);
            }
        }

        private void OnRouteFail(Indication indication, Indication prevIndication)
        {
            AudioManager.Instance.PlaySound("wrong_answer");
            this._dialogueManager.FailDialogue(indication, prevIndication);
            this.RemoveLife();
        }

        private void RemoveLife()
        {
            AudioManager.Instance.PlaySound("wrong_answer");

            this._livesCount--;
            if (this._livesCount <= 0) {
                this._dialogueManager.NextCharacter();
                this._livesCount = this._livesTotal;
            }

            this._guiManager.SetStarsCount(this._livesCount);
        }
        
        public void AddLife()
        {
            AudioManager.Instance.PlaySound("correct_answer");

            this._livesCount++;
            if (this._livesCount > this._livesTotal) {
                this._livesCount = this._livesTotal;
            }

            this._guiManager.SetStarsCount(this._livesCount);
        }

        private void OnDrawGizmos()
        {
            this._planner?.OnDrawGizmos();
        }

        private void Update()
        {
            this._planner.UpdatePath();

            if (Input.GetKeyDown(KeyCode.Space)) {
                this._dialogueManager?.NextCharacter();
            }
        }
    }
}