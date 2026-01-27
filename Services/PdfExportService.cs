using Microsoft.JSInterop;

namespace DevJournal.Services;

public class PdfExportService
{
    private readonly IJSRuntime _js;

    public PdfExportService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task ExportEntriesAsync(List<JournalEntry> entries, string title)
    {
        var htmlContent = "<div class='journal-export'>";
        htmlContent += $"<h1>{title}</h1>";
        htmlContent += $"<p class='meta'>Generated on {DateTime.Now:F}</p><hr/>";

        foreach (var entry in entries)
        {
            htmlContent += $@"
                <div class='entry'>
                    <h2>{entry.Title}</h2>
                    <p class='meta'>
                        <strong>Date:</strong> {entry.Date:yyyy-MM-dd} | 
                        <strong>Mood:</strong> {entry.PrimaryMood}
                    </p>
                    <div>{entry.Content}</div>
                </div>";
        }

        htmlContent += "</div>";

        await _js.InvokeVoidAsync("downloadPdf", $"Journal_Export_{DateTime.Now:yyyyMMdd}.pdf", htmlContent);
    }
}
