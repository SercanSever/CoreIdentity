using Identity.DataAccess.Abstract;
using Identity.DataAccess.EntityFrameworkDal;
using Identity.Entity;

namespace Identity.DataAccess.Concrete
{
    public class UserDal : EntityRepositoryBase<User>, IUserDal
    {

    }
}