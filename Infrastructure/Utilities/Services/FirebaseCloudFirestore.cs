using Application.Contracts;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utilities.Services;

public class FirebaseCloudFirestore : IFirebaseCloudFirestore
{
    private readonly FirestoreDb _db;
    public FirebaseCloudFirestore(IApplicationConfiguration config)
    {
        var firebaseConfiguration = config.GetFirebaseSettings();
        var privateKeyValue = firebaseConfiguration.private_key.Replace("\\n", "\n");
        firebaseConfiguration.private_key = privateKeyValue;

        var jsonFormat = JsonSerializer.Serialize(firebaseConfiguration);

        var firestoreClient = new FirestoreClientBuilder()
        {
            JsonCredentials = jsonFormat
        }.Build();

        _db = FirestoreDb.Create(firebaseConfiguration.project_id, firestoreClient);
    }

    public async Task<T> Get<T>(string collection, string id)
    {
        DocumentReference document = _db.Collection(collection).Document(id);

        DocumentSnapshot snapshot = await document.GetSnapshotAsync();

        return snapshot.ConvertTo<T>();
    }

    public async Task<IEnumerable<T>> GetAll<T>(string collection)
    {
        var query = _db.Collection(collection);

        QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

        return querySnapshot.Documents.Select(doc => doc.ConvertTo<T>());
    }

    public async Task Add(string collection, object data, string id = null)
    {
        if (string.IsNullOrWhiteSpace(id))
            await _db.Collection(collection).AddAsync(data);
        else
            await _db.Collection(collection).Document(id).SetAsync(data);
    }

    public async Task Update(string collection, string id, object data)
    {
        DocumentReference document = _db.Collection(collection).Document(id);

        await document.SetAsync(data);
    }

    public async Task Delete(string collection, string id)
    {
        DocumentReference document = _db.Collection(collection).Document(id);

        await document.DeleteAsync();
    }
}