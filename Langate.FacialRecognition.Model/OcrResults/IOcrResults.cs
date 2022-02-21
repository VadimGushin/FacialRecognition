using System.Threading.Tasks;

namespace Langate.FacialRecognition.Model
{
    public interface IOcrResults
    {
        Task Add(OcrResults ocrResult);

        Task<OcrResults> WithId(int id);
    }
}
