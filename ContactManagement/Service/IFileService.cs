namespace ContactManagement.Service;
internal interface IFileService
{
    public bool Export<T>(string filepath, IEnumerable<T> data);

}
