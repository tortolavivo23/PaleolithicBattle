abstract class BTNode
{
    public abstract BTState Tick(IUnit unit);
}

enum BTState { Success, Failure, Running }
