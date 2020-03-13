using UnityEngine;

namespace ScarabPuzzle {

    public class NodeActiveState : INodeState {
        public void Execute(NodeController node) {
            node.GetComponent<MeshRenderer>().material = Resources.Load("Materials/NodeActive", typeof(Material)) as Material;
            node.NodeState = NodeState.Active;
        }
    }
}
