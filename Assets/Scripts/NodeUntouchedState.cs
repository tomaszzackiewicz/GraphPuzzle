using UnityEngine;

namespace ScarabPuzzle {

    public class NodeUntouchedState : INodeState {
        public void Execute(NodeController node) {
            node.GetComponent<MeshRenderer>().material = Resources.Load("Materials/NodeUntouched", typeof(Material)) as Material;
            node.NodeState = NodeState.Untouched;
        }
    }
}
