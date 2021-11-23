using Identity.DataAccess.Concrete;
using Identity.DataAccess.EntityFrameworkDal;
using Identity.Entity;

namespace Identity.DataAccess.Abstract
{
    public interface IRoleDal : IEntityRepository<Role>
    {

    }
}