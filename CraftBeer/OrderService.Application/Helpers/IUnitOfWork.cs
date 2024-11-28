using System.Data;

namespace OrderService.Application.Helpers
{
    //TODO: Implement Unit of Work on activities
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Serializable);
    }
}
