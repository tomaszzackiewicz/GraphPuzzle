using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScarabPuzzle {

    public class NodeController : MonoBehaviour {

        public Image scarabImage;
        public List<LinkController> links = new List<LinkController>();
        public List<NodeController> neighbors = new List<NodeController>();
        public AudioClip failureSound;
        public AudioClip successSound;

        private INodeState _theState;
        private AudioSource _audioSource;
        private ParticleSystem _activeNodeParticlesPrefab;
        private Sprite _stoneScarab;
        private Sprite _silverScarab;
        private Sprite _goldScarab;

        public NodeState NodeState { get; set; }
        public bool IsNodeClicked { get; set; }

        void Awake() {
            _audioSource = gameObject.GetComponent<AudioSource>();

            _activeNodeParticlesPrefab = Resources.Load("Particles/ActiveNode", typeof(ParticleSystem)) as ParticleSystem;
            _stoneScarab = Resources.Load("Textures/ScarabStone", typeof(Sprite)) as Sprite;
            _silverScarab = Resources.Load("Textures/ScarabSilver", typeof(Sprite)) as Sprite;
            _goldScarab = Resources.Load("Textures/ScarabGold", typeof(Sprite)) as Sprite;
        }

        void OnEnable() {
            ScarabCharacterController.resetNodes += ResetNode;
        }

        void Start() {
            InitializeNode();
        }

        public void InitializeNode() {
            scarabImage.sprite = _stoneScarab;
            this.gameObject.tag = "Neighbor";
            IsNodeClicked = false;
            NodeUntouched();
            NodeState = NodeState.Untouched;
        }

        public void NodeUntouched() {
            scarabImage.sprite = _stoneScarab;
            _theState = new NodeUntouchedState();
            _theState.Execute(this);
        }

        public void NodeTouched() {
            scarabImage.sprite = _silverScarab;
            _theState = new NodeTouchedState();
            _theState.Execute(this);
        }

        public void NodeActive() {
            scarabImage.sprite = _goldScarab;
            _theState = new NodeActiveState();
            _theState.Execute(this);
        }

        void ResetNode() {
            if (NodeState == NodeState.Active) {
                NodeTouched();
                NodeState = NodeState.Touched;
            }
        }

        void OnDisable() {
            ScarabCharacterController.resetNodes -= ResetNode;
        }

        public bool CheckAllLinksStateUntouchedActive() {
            bool isAnyUntouched = false;
            foreach (LinkController link in links) {
                if (link.LinkState == LinkState.Untouched) {
                    isAnyUntouched = true;
                }
            }

            return isAnyUntouched;
        }

        public bool CheckAllLinksStateUntouched() {
            foreach (LinkController link in links) {
                if (link.LinkState == LinkState.Untouched) {
                    return true;
                }
            }
            return false;
        }

        public void ForbiddenNodeSound() {
            if (!_audioSource.isPlaying) {
                _audioSource.clip = failureSound;
                _audioSource.Play();
            }
        }

        public void SuccessNodeSound() {
            if (!_audioSource.isPlaying) {
                _audioSource.clip = successSound;
                _audioSource.Play();
            }
        }

        public void PlayActiveNodeParticles() {
            ParticleSystem particleSystem1 = Instantiate(_activeNodeParticlesPrefab, gameObject.transform.position, gameObject.transform.rotation) as ParticleSystem;
            particleSystem1.Play();
        }
    }
}
