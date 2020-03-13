using UnityEngine;

namespace ScarabPuzzle {

    public class TriggerController : MonoBehaviour {

        public TriggerType triggerType;

        void OnTriggerEnter(Collider col) {
            if (col.gameObject.CompareTag("Player")) {
                PuzzleManager.Instance.SetTriggerActive(triggerType);
            }
        }

        void OnTriggerExit(Collider col) {
            if (col.gameObject.CompareTag("Player")) {
                PuzzleManager.Instance.SetTriggerDeactive();
            }
        }
    }

}
