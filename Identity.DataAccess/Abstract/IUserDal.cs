using Identity.DataAccess.EntityFrameworkDal;
using Identity.Entity;

namespace Identity.DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {

    }
}