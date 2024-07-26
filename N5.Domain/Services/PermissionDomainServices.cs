using N5.Domain.Interfaces;
namespace N5.Domain.Services;
public class PermissionDomainServices : IPermissionDomainServices
{
    public bool ValidateDatePermission(DateTime datePermission)
    {
        return datePermission < DateTime.Now ? false : true;
    }
}
