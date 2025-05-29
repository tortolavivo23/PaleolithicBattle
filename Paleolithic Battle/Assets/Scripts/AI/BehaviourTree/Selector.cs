using System.Collections.Generic;
using System.Linq;

class Selector : BTNode
{
    private List<BTNode> children;
    private Dictionary<IUnit, int> runningChildIndex = new();

    public Selector(params BTNode[] nodes) => children = nodes.ToList();

    public override BTState Tick(IUnit unit)
    {
        if (!runningChildIndex.ContainsKey(unit))
            runningChildIndex[unit] = 0;

        for (int i = runningChildIndex[unit]; i < children.Count; i++)
        {
            var state = children[i].Tick(unit);

            if (state == BTState.Running)
            {
                runningChildIndex[unit] = i;
                return BTState.Running;
            }

            if (state == BTState.Success)
            {
                runningChildIndex.Remove(unit);
                return BTState.Success;
            }

            // Si falla, seguir con el siguiente hijo
        }

        runningChildIndex.Remove(unit);
        return BTState.Failure;
    }
}
