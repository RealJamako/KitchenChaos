using System;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;

    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }

    public override void Interact(PlayerController ply)
    {
        if (!ply.HasKitchenObject()) { return; }
        OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        ply.KitchenObject.DestorySelf();
    }
}