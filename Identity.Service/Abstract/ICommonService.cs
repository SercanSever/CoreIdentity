using Identity.Service.Enums;

namespace Identity.Service.Abstract
{
    public interface ICommonService
    {
       string ShowAlert(Alerts alert,string message);
    }
}