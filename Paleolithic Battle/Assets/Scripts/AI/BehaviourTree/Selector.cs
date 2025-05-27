using System.Collections.Generic;
using System.Linq;

class Selector : BTNode
{
    private List<BTNode> children;
    public Selector(params BTNode[] nodes) => children = nodes.ToList();

    public override BTState Tick(IUnit unit)
    {
        foreach (var child in children)
        {
            var state = child.Tick(unit);
            if (state != BTState.Failure)
                return state;
        }
        return BTState.Failure;
    }
}