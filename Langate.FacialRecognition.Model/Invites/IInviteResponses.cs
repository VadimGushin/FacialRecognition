using System.Threading.Tasks;

namespace Langate.FacialRecognition.Model
{
    public interface IInviteResponses
    {
        Task Add(InviteResponse inviteResponse);
        void Update(InviteResponse inviteResponse);
        Task<InviteResponse> WithId(int id);
    }
}
