namespace HospitalManager.Domain.Repositories;
public interface IUnityOfWork
{
    public Task Commit();
}
