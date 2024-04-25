namespace ContactManagement.Service;
public interface IFileService
{
    public bool Export<T>(string filepath, IEnumerable<T> data);

}
