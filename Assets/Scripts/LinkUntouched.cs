using UnityEngine;

namespace ScarabPuzzle {

    public class LinkUntouched : ILinkState {
        public void Execute(LinkController node) {
            node.GetComponent<MeshRenderer>().material = Resources.Load("Materials/NodeUntouched", typeof(Material)) as Material;
            node.LinkState = LinkState.Untouched;
        }
    }
}
