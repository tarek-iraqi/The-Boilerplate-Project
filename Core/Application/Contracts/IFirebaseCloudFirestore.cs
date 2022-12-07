using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contracts;

public interface IFirebaseCloudFirestore
{
    Task<T> Get<T>(string collection, string id);

    Task<IEnumerable<T>> GetAll<T>(string collection);

    Task Add(string collection, object data, string id = null);

    Task Update(string collection, string id, object data);

    Task Delete(string collection, string id);
}