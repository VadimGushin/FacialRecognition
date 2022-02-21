using System.Threading.Tasks;

namespace Langate.FacialRecognition.Model
{
    public interface IInvites
    {
        Task<Invite> WithId(int id);
    }
}
