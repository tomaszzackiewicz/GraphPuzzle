using UnityEngine;

namespace ScarabPuzzle {

    public class PuzzleController : MonoBehaviour {

        public GameObject scarab;
        public ScarabGameManager ScarabGameManager { get; set; }

        void Awake() {
            ScarabGameManager = scarab.GetComponent<ScarabGameManager>();
        }

        public void ActivateScarab(bool isActivated) {
            scarab.SetActive(isActivated);
        }

    }
}
