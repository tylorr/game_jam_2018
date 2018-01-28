using UniRx;
using UnityEngine;

public static class Extensions
{
    public static CoroutineAsyncBridge GetAwaiter(this YieldInstruction yieldInstruction)
    {
        return CoroutineAsyncBridge.Start(yieldInstruction);
    }
}
