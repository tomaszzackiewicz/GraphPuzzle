using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace ScarabPuzzle {

    public class ScarabCharacterController : MonoBehaviour {

        private static ScarabCharacterController _instance;
        public static ScarabCharacterController Instance { get { return _instance; } }

        public delegate void ResetNodes();
        public static event ResetNodes resetNodes;

        public FirstPersonController firstPersonController;
        public float distance = 10.0f;
        public float resetDelay = 2.0f;

        private RaycastHit _hit;

        public NodeController LastNodeController { get; set; } = null;
        public bool IsClickBlocked { get; set; } = false;

        void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            } else {
                _instance = this;
            }
        }

        void Update() {
            if (!IsClickBlocked) {
                if (Input.GetMouseButtonDown(0)) {
                    if (Physics.Raycast(this.transform.position, this.transform.forward, out _hit, distance)) {
                        if (_hit.collider.gameObject) {
                            if ((_hit.collider.gameObject.CompareTag("Node") || _hit.collider.gameObject.CompareTag("Neighbor"))) {
                                if (resetNodes != null) {
                                    resetNodes();
                                }
                                NodeController freshNodeController = _hit.collider.gameObject.GetComponent<NodeController>();
                                if (LastNodeController) {
                                    if (!GameObject.ReferenceEquals(freshNodeController, LastNodeController)) {

                                        foreach (LinkController link1 in freshNodeController.links) {

                                            foreach (LinkController link2 in LastNodeController.links) {
                                                if (link1 == link2) {
                                                    if (link1.LinkState == LinkState.Untouched) {
                                                        LastNodeController = freshNodeController;
                                                        LastNodeController.SuccessNodeSound();
                                                        LastNodeController.PlayActiveNodeParticles();
                                                    } else {
                                                        LastNodeController.ForbiddenNodeSound();
                                                    }
                                                }
                                            }
                                        }
                                    }

                                } else {
                                    LastNodeController = freshNodeController;
                                    LastNodeController.SuccessNodeSound();
                                    LastNodeController.PlayActiveNodeParticles();
                                }
                                LastNodeController.NodeActive();

                                LastNodeController.IsNodeClicked = true;
                                PuzzleManager.Instance.ScarabGameManager.HighlightLink();
                                StartCoroutine(CheckLinksCor());

                            } else if (_hit.collider.gameObject.CompareTag("Untagged")) {
                                NodeController lastNodeControllerUntagged = _hit.collider.gameObject.GetComponent<NodeController>();
                                if (lastNodeControllerUntagged) {
                                    lastNodeControllerUntagged.ForbiddenNodeSound();
                                }
                            }
                        }
                    }
                }
            }

        }

        IEnumerator CheckLinksCor() {
            yield return new WaitForSeconds(0.1f);
            if (!LastNodeController.CheckAllLinksStateUntouchedActive()) {
                PuzzleManager.Instance.ScarabGameManager.DisableNodes();
                IsClickBlocked = true;
                ActivateFirstPersonController(false);
                PuzzleManager.Instance.ScarabGameManager.SetText(true);
                StartCoroutine(ResetPuzzleCor());
            } else {
                PuzzleManager.Instance.ScarabGameManager.GetNeighborsOnly(LastNodeController);
            }
        }

        IEnumerator ResetPuzzleCor() {
            yield return new WaitForSeconds(2.0f);
            PuzzleManager.Instance.ScarabGameManager.ResetPuzzle();
            ActivateFirstPersonController(true);
            PuzzleManager.Instance.ScarabGameManager.SetText(false);
        }

        public void ActivateFirstPersonController(bool isSPSActivated) {
            firstPersonController.enabled = isSPSActivated;
        }
    }
}
