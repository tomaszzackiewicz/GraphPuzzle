using UnityEngine;

namespace ScarabPuzzle {
    public class LinkTouched : ILinkState {
        public void Execute(LinkController node) {
            node.GetComponent<MeshRenderer>().material = Resources.Load("Materials/NodeTouched", typeof(Material)) as Material;
            node.LinkState = LinkState.Touched;
        }
    }
}
