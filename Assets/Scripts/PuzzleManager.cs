using UnityEngine;

namespace ScarabPuzzle {

    public class PuzzleManager : MonoBehaviour {

        private static PuzzleManager _instance;
        public static PuzzleManager Instance { get { return _instance; } }

        public PuzzleController frontPuzzle;
        public PuzzleController backPuzzle;
        public PuzzleController leftPuzzle;
        public PuzzleController rightPuzzle;

        public ScarabGameManager ScarabGameManager { get; set; }

        void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            } else {
                _instance = this;
            }
        }

        void Update() {// For DEBUG only - can be removed
            if (Input.GetKeyDown(KeyCode.P)) {
                ScarabGameManager.DebugWinning();
            }
        }

        public void SetTriggerActive(TriggerType triggerType) {
            switch (triggerType) {
                case TriggerType.Front:
                    ScarabGameManager = frontPuzzle.ScarabGameManager;
                    break;
                case TriggerType.Back:
                    ScarabGameManager = backPuzzle.ScarabGameManager;
                    break;
                case TriggerType.Left:
                    ScarabGameManager = leftPuzzle.ScarabGameManager;
                    break;
                case TriggerType.Right:
                    ScarabGameManager = rightPuzzle.ScarabGameManager;
                    break;
            }
        }

        public void SetTriggerDeactive() {
            ScarabGameManager.ResetPuzzle();
        }
    }
}
