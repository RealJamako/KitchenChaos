using System;
public interface IHasProgress
{
    public event EventHandler<OnCounterProgressChangedArgs> OnProgressChanged;
}