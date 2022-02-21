using System.Threading.Tasks;

namespace Langate.FacialRecognition.Model
{
    public interface IApiCalls
    {
        Task Add(ApiCall apiCall);
    }
}
