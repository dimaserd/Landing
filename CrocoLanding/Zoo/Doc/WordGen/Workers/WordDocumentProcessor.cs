using Croco.Core.Models;
using System;
using Zoo.Doc.WordGen.Abstractions;
using Zoo.Doc.WordGen.Models;

namespace Zoo.Doc.WordGen.Workers
{
    public class WordDocumentProcessorOptions
    {
        public IWordProccessorEngine Engine { get; set; }
    }

    public class WordDocumentProcessor
    {
        public WordDocumentProcessor(WordDocumentProcessorOptions options)
        {
            Engine = options.Engine;
        }

        IWordProccessorEngine Engine { get; }

        public BaseApiResponse RenderDocument(DocXDocumentObjectModel model)
        {
            try
            {
                Engine.Create(model);
                return new BaseApiResponse(true, "Документ создан");
            }
            catch (Exception ex)
            {
                return new BaseApiResponse(ex);
            }
        }
    }
}