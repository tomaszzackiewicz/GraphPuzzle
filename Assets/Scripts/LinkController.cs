using UnityEngine;

namespace ScarabPuzzle {

    public class LinkController : MonoBehaviour {

        public NodeController nodeStartController;
        public NodeController nodeEndController;

        private ILinkState _theState;

        public LinkState LinkState { get; set; }

        void Start() {

            InitializeLink();
        }

        public void InitializeLink() {
            LinkUntouched();
            LinkState = LinkState.Untouched;
            nodeStartController = null;
            nodeEndController = null;
        }

        public void LinkUntouched() {
            _theState = new LinkUntouched();
            _theState.Execute(this);
        }

        public void LinkTouched() {
            _theState = new LinkTouched();
            _theState.Execute(this);
        }

        //void CheckLineState() {

        //    if (nodeStartController.IsNodeClicked && nodeEndController.IsNodeClicked) {
        //        if (GameObject.ReferenceEquals(nodeStartController, nodeEndController)) {
        //            LinkTouched();
        //        }
        //    } else {
        //        LinkUntouched();
        //    }
        //}

    }
}
