using UnityEngine;

using strange.extensions.command.impl;

public class ReplayCommand : EventCommand
{
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        dispatcher.Dispatch(NotificationConst.Noti_ContinueGame);

    }
}