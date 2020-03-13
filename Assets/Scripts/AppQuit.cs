using UnityEngine;

namespace ScarabPuzzle {

    public class AppQuit : MonoBehaviour {

        void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                Application.Quit();
            }
        }
    }
}
