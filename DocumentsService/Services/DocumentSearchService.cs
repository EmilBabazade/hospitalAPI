using Grpc.Core;

namespace DocumentsService.Services;

public class DocumentSearchService : DocumentSearch.DocumentSearchBase
{
    // Simulating a data store
    private static readonly List<Document> Documents =
    [
        new Document { Id = "1", PatientId = "987e6543-e21b-34d3-c456-426614174111", Name = "Document1" },
        new Document { Id = "2", PatientId = "987e6543-e21b-34d3-c456-426614174111", Name = "Document2" }
    ];
    public override Task<DocumentList> GetAll(PatientId request,
        ServerCallContext context)
    {
        var documentList = new DocumentList();
        documentList.Documents.AddRange(Documents.Where(
            q => q.PatientId == request.Id));
        return Task.FromResult(documentList);
    }
    public override Task<Document> Get(DocumentId request,
        ServerCallContext context)
    {
        var document = Documents.Find(doc => doc.Id == request.Id);
        if (document == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound,
                "Document not found"));
        }
        return Task.FromResult(document);
    }
}