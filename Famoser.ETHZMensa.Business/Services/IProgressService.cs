namespace Famoser.ETHZMensa.Business.Services
{
    public interface IProgressService
    {
        void InitializeProgressBar(int total);
        void IncrementProgress();
        void HideProgress();
    }
}
