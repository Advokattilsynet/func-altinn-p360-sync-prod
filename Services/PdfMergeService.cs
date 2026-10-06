using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace func_altinn_p360_sync_prod.Services;

public class PdfMergeService
{
    private readonly ILogger<PdfMergeService> _logger;

    public PdfMergeService(ILogger<PdfMergeService> logger)
    {
        _logger = logger;
    }

    public byte[]? MergePdfDocuments(IEnumerable<(string Filename, byte[] Bytes)>? pdfFiles)
    {
        if (pdfFiles == null)
        {
            return null;
        }

        var fileList = pdfFiles.Where(f => f.Bytes != null && f.Bytes.Length > 0).ToList();

        if (fileList.Count == 0)
        {
            return null;
        }

        if (fileList.Count == 1)
        {
            _logger.LogInformation($"Only one PDF found ({fileList[0].Filename}), no merging required.");
            return fileList[0].Bytes;
        }

        // Place the last file (the filled out form) first, followed by the preceding files in their original order
        var reorderedList = new List<(string Filename, byte[] Bytes)> { fileList[^1] };
        reorderedList.AddRange(fileList.Take(fileList.Count - 1));

        _logger.LogInformation($"Merging {reorderedList.Count} PDFs into a single document. First file placed: {reorderedList[0].Filename}");

        using var outputDocument = new PdfDocument();

        for (int fileIndex = 0; fileIndex < reorderedList.Count; fileIndex++)
        {
            var file = reorderedList[fileIndex];

            try
            {
                using var stream = new MemoryStream(file.Bytes);
                using var inputDocument = PdfReader.Open(stream, PdfDocumentOpenMode.Import);

                PdfPage? firstPage = null;

                for (int i = 0; i < inputDocument.PageCount; i++)
                {
                    var page = outputDocument.AddPage(inputDocument.Pages[i]);

                    if (i == 0)
                    {
                        firstPage = page;
                    }
                }

                if (firstPage != null)
                {
                    var bookmarkTitle = GetBookmarkTitle(file.Filename, fileIndex + 1);
                    outputDocument.Outlines.Add(bookmarkTitle, firstPage);

                    _logger.LogInformation($"Added bookmark '{bookmarkTitle}' pointing to page {outputDocument.PageCount - inputDocument.PageCount + 1}");
                }

                _logger.LogInformation($"Appended {inputDocument.PageCount} pages from {file.Filename}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to append PDF {file.Filename} during merge.");
                throw;
            }
        }

        using var outputStream = new MemoryStream();
        outputDocument.Save(outputStream, false);
        return outputStream.ToArray();
    }

    private static string GetBookmarkTitle(string filename, int index)
    {
        var name = Path.GetFileNameWithoutExtension(filename);

        if (string.IsNullOrWhiteSpace(name))
        {
            name = "Dokument";
        }

        return $"{index}. {name}";
    }
}
