using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ScarabPuzzle {

    public class ScarabGameManager : MonoBehaviour {

        public Transform parentParticle;

        public Image textBG;
        public TextMeshProUGUI resultText;
        public Image crosshair;

        public AudioClip loserSound;
        public AudioClip winnerSound;

        public List<NodeController> nodes = new List<NodeController>();
        public List<LinkController> links = new List<LinkController>();

        private ParticleSystem _particleWinnerPrefab;
        private AudioSource _audioSource;
        private NodeController _first;
        private NodeController _second;

        void Awake() {

            _audioSource = gameObject.GetComponent<AudioSource>();

            _particleWinnerPrefab = Resources.Load("Particles/WinnerLightning", typeof(ParticleSystem)) as ParticleSystem;
        }

        void Start() {
            ResetPuzzle();
        }

        public void ResetPuzzle() {
            ScarabCharacterController.Instance.LastNodeController = null;
            SetTextLoser(false);
            SetTextWinner(false);
            SetCrosshair(true);
            _first = null;
            _second = null;
            InitializeNodes();
            InitializeLinks();

        }

        void InitializeNodes() {
            foreach (NodeController node in nodes) {
                node.InitializeNode();
            }
        }

        void InitializeLinks() {
            foreach (LinkController link in links) {
                link.InitializeLink();
            }
        }

        public void DisableNodes() {
            foreach (NodeController node in nodes) {
                node.gameObject.tag = "Untagged";
            }
        }

        public void HighlightLink() {

            if (_first != null) {
                _second = _first;
                _first = null;
            }

            if (_first == null) {
                foreach (NodeController node in nodes) {
                    if (node.IsNodeClicked) {
                        if (node != _second) {
                            _first = node;
                        }
                    }
                }
            }

            StartCoroutine(GetFirstAndSecondCor());

        }

        IEnumerator GetFirstAndSecondCor() {
            yield return new WaitForSeconds(0.01f);
            CompareLinks();
        }

        void CompareLinks() {

            if (_first && _second) {
                foreach (LinkController link1 in _first.links) {
                    foreach (LinkController link2 in _second.links) {
                        if (link1 == link2) {
                            link1.LinkTouched();
                        }
                    }
                }
            }
            if (_first) {
                ResetClickedNodes();
            }
        }

        private void ResetClickedNodes() {
            foreach (NodeController node in nodes) {
                if (!GameObject.ReferenceEquals(node.gameObject, _first.gameObject)) {
                    node.IsNodeClicked = false;
                }
            }
            StartCoroutine(CheckNodeLinksCor());
        }

        IEnumerator CheckNodeLinksCor() {
            yield return new WaitForSeconds(0.01f);
            foreach (NodeController node in nodes) {
                if (!node.CheckAllLinksStateUntouched()) {
                    node.gameObject.tag = "Untagged";
                }
            }
        }

        public void GetNeighborsOnly(NodeController nodeParam) {
            foreach (NodeController node in nodes) {
                node.gameObject.tag = "Untagged";
            }
            foreach (NodeController node in nodeParam.neighbors) {
                node.gameObject.tag = "Neighbor";
            }
            nodeParam.gameObject.tag = "Active";
        }

        public void SetText(bool isShown) {
            if (CheckIfWinner()) {
                SetTextLoser(isShown);
            } else {
                SetTextWinner(isShown);
            }
        }

        void SetTextLoser(bool isShown) {
            resultText.text = "Loser!";
            textBG.gameObject.SetActive(isShown);
            SetCrosshair(!isShown);

            if (isShown) {
                _audioSource.PlayOneShot(loserSound);
                StartCoroutine(UnblockNewPathSelectingCor(3.0f));
            }
        }

        void SetTextWinner(bool isShown) {
            resultText.text = "Winner!";
            textBG.gameObject.SetActive(isShown);
            SetCrosshair(!isShown);
            if (isShown) {
                ParticleSystem particleSystem1 = Instantiate(_particleWinnerPrefab, parentParticle.transform.position, parentParticle.transform.rotation) as ParticleSystem;
                particleSystem1.Play();
                particleSystem1.GetComponentInChildren<ParticleSystem>().Play();
                _audioSource.PlayOneShot(winnerSound);
                StartCoroutine(UnblockNewPathSelectingCor(7.0f));
            }
        }

        void SetCrosshair(bool isShown) {
            if (isShown) {
                crosshair.gameObject.SetActive(isShown);
            } else {
                crosshair.gameObject.SetActive(isShown);
            }
        }

        IEnumerator UnblockNewPathSelectingCor(float time) {
            yield return new WaitForSeconds(time);
            ScarabCharacterController.Instance.IsClickBlocked = false;
        }

        public bool CheckIfWinner() {
            bool isAnyUntouched = false;
            foreach (LinkController link in links) {
                if (link.LinkState == LinkState.Untouched) {
                    isAnyUntouched = true;
                }
            }
            return isAnyUntouched;
        }

        public void DebugWinning() {
            ////////////////////////////////////////////////DEBUG - can be removed

            foreach (NodeController node in nodes) {
                if (node.name == "Node1") {
                    node.NodeUntouched();
                    node.gameObject.tag = "Neighbor";
                } else if (node.name == "Node2") {
                    node.NodeActive();
                    _second = node;
                } else {
                    node.NodeTouched();
                }
            }
            foreach (LinkController link in links) {
                if (link.name == "Link1") {
                    link.LinkUntouched();
                } else {
                    link.LinkTouched();
                }
            }

            ////////////////////////////////////////////////DEBUG - can be removed 
        }

    }
}
