using UnityEngine;

namespace ScarabPuzzle {

    public class NodeTouchedState : INodeState {
        public void Execute(NodeController node) {
            node.GetComponent<MeshRenderer>().material = Resources.Load("Materials/NodeTouched", typeof(Material)) as Material;
            node.NodeState = NodeState.Touched;
            node.tag = "Untagged";
        }
    }
}