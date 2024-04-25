using CsvHelper;
using System.Globalization;

namespace ContactManagement.Service;
public class CsvFileService : IFileService
{
    public bool Export<T>(string filepath, IEnumerable<T> data)
    {
        try
        {
            using var writer = new StreamWriter(filepath);
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(data);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
}
