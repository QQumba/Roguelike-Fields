using System.Threading.Tasks;

namespace Animations.AsyncAnimations
{
    public interface IAsyncAnimation
    {
        Task Play();
        void RequestStop();
    }
}