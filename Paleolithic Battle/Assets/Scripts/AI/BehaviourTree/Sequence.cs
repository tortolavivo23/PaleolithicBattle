using System.Collections.Generic;
using System.Linq;

class Sequence : BTNode
{
    private List<BTNode> children;
    public Sequence(params BTNode[] nodes) => children = nodes.ToList();

    public override BTState Tick(IUnit unit)
    {
        foreach (var child in children)
        {
            var state = child.Tick(unit);
            if (state != BTState.Success)
                return state;
        }
        return BTState.Success;
    }
}