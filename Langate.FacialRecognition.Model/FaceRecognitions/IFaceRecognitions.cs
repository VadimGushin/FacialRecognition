using System.Threading.Tasks;

namespace Langate.FacialRecognition.Model
{
    public interface IFaceRecognitions
    {
        Task Add(FaceRecognition faceRecognition);
    }
}
