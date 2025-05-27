using UnityEngine;

class WaitNode : BTNode
{
    private float waitTime;
    private float elapsedTime = 0f;
    private bool isWaiting = false;

    public WaitNode(float time)
    {
        waitTime = time;
    }

    public override BTState Tick(IUnit unit)
    {
        if (!isWaiting)
        {
            isWaiting = true;
            elapsedTime = 0f;
            return BTState.Running;
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= waitTime)
        {
            isWaiting = false;
            return BTState.Success;
        }

        return BTState.Running;
    }
}