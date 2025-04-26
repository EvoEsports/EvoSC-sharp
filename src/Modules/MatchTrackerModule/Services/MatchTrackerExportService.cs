using System.Text;
using EvoSC.Common.Services.Attributes;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchTrackerModule.Services;

[Service]
public class MatchTrackerExportService(IMatchRecordRepository repository) : IMatchTrackerExportService
{
    public async Task ExportCsv(string file)
    {
        var records = await repository.GetRecordsAsync();
        var sb = new StringBuilder();

        sb.AppendLine("Id,Timestamp,TimelineId,Status,Report");

        foreach (var record in records)
        {
            sb.Append(record.Id);
            sb.Append(",");
            sb.Append(record.Timestamp);
            sb.Append(",");
            sb.Append(record.TimelineId);
            sb.Append(",");
            sb.Append(record.Status);
            sb.Append(",\"");
            sb.Append(EscapeCsvValue(record.Report));
            sb.AppendLine("\"");
        }

        await File.WriteAllTextAsync(file, sb.ToString());
    }

    private static string EscapeCsvValue(string value)
    {
        return value.Replace("\"", "\"\"");
    }
}
