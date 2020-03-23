using Zoo.Doc.WordGen.Models;

namespace Zoo.Doc.WordGen.Abstractions
{
    public interface IWordProccessorEngine
    {
        void Create(DocXDocumentObjectModel model);
    }
}
