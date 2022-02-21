using System.Threading.Tasks;

namespace Langate.FacialRecognition.Model
{
    public interface IImages
    {
        Task<Image> WithId(int id);
        Task Add(Image image);
        void MarkDeleted(Image image);
    }
}
