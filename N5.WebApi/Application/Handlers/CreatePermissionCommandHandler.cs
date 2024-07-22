using MediatR;
using N5.Infraestructure.Interfaces;
using N5.WebApi.Application.Commands.Permissions;

namespace N5.WebApi.Application.Handlers;

public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, bool>
{
    #region privateMembers 
    private readonly IUnitofWork _unitofWork;
    #endregion

    public CreatePermissionCommandHandler(IUnitofWork unitOfWork)
    {
        _unitofWork = unitOfWork;
    }

    public async Task<bool> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Domain.Entities.Permisos permiso = new Domain.Entities.Permisos();
            permiso.NombreEmpleado = request.EmployeeName;
            permiso.ApellidoEmpleado = request.EmployeeSurname;
            permiso.FechaPermiso = DateOnly.FromDateTime(request.PermissionDate);
            permiso.TiposPermisoId = request.PermissionType;
            _unitofWork.PermisosRepository.Add(permiso);
            
        }

        catch (Exception ex)
        {

        }
        return true;
    }
}
