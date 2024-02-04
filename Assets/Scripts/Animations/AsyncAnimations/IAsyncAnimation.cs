using System.Collections;
using System.Threading.Tasks;
using Cells;

namespace Animations.AsyncAnimations
{
    public interface IAsyncAnimation
    {
        Task PlayAsync();
        IEnumerator Play();
        IEnumerator Play(Cell cell);
        void RequestStop();
    }
}