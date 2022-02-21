using System.Threading.Tasks;

namespace Langate.FacialRecognition.Model
{
    public interface IPersons
    {
        Task Add(Person person);
        Task<Person> WithSubjectId(int subjectId);
        Task<Person> WithAzurePersonId(string azurePersonId);
    }
}
