using System.Threading.Tasks;

namespace Langate.FacialRecognition.Model
{
    public interface IEndUserAgreements
    {
        Task<EndUserAgreement> WithStudyId(int studyId);
    }
}
