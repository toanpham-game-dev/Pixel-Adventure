/// <summary>
/// Base interface for all Behavior Tree nodes.
/// Every node must implement the Tick method, which is called
/// every update to evaluate the node's current state.
/// </summary>
public enum NodeState
{
    Success,
    Failure,
    Running
}