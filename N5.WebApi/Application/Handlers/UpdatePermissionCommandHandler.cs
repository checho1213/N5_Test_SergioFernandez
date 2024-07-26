using MediatR;
using Microsoft.Extensions.Options;
using N5.Domain.Entities;
using N5.Domain.Exceptions;
using N5.Domain.Interfaces;
using N5.Infraestructure.Interfaces;
using N5.Infraestructure.Settings;
using N5.WebApi.Application.Commands.Permissions;
using N5.WebApi.dto;
using Newtonsoft.Json;

namespace N5.WebApi.Application.Handlers;

public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, ResponseMessageDto<Permission>>
{
    #region privateMembers 
    private readonly IUnitofWork _unitofWork;
    private readonly IKafkaRepository _kafkaRepository;
    private readonly string permissionEventTopic;
    private readonly IPermissionDomainServices _permissionDomainServices;
    #endregion

    public UpdatePermissionCommandHandler(IUnitofWork unitOfWork, IKafkaRepository kafkaRepository, IOptions<InfraestructureSettings> infraSettings, IPermissionDomainServices permissionDomainServices)
    {
        _unitofWork = unitOfWork;
        _kafkaRepository = kafkaRepository;
        permissionEventTopic = infraSettings.Value.KafkaSettings.Topics.PermissionEvent;
        _permissionDomainServices = permissionDomainServices;
    }

    public async Task<ResponseMessageDto<Permission>> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        int statusCode = StatusCodes.Status500InternalServerError;
        try
        {
            Domain.Entities.Permission permiso;
            permiso = _unitofWork.PermisosRepository.GetById(request.IdPermission);
            if (permiso == null)
            {
                statusCode = StatusCodes.Status404NotFound;
                return BuildMessage(statusCode, null, "El permiso enviado no existe");
            }
            var resultValidation = _permissionDomainServices.ValidateDatePermission(request.PermissionDate);
            if (!resultValidation)
            {
                statusCode = StatusCodes.Status400BadRequest;
                throw new DomainExceptions("La fecha no valida.", new Exception("La fecha del permiso no puede ser inferior a la fecha actual."));
            }

            permiso.Date = DateOnly.FromDateTime(request.PermissionDate);
            permiso.PemissionTypeId = request.PermissionType;
            _unitofWork.PermisosRepository.Update(permiso);
            await SendMessageToPermissionEventsTopic(permiso);
            return BuildMessage(StatusCodes.Status200OK, permiso, "Permiso actualizado con exito");
        }
        catch (Exception ex)
        {
            return BuildMessage(statusCode, null, ex.InnerException.Message);
        }
    }

    private async Task SendMessageToPermissionEventsTopic(Permission permission)
    {
        PermissionEnventDto message = new PermissionEnventDto();
        message.Operation = "Modify";
        message.Message = JsonConvert.SerializeObject(permission);
        message.Id = Guid.NewGuid();
        await _kafkaRepository.SendMessageToTopic(permissionEventTopic, JsonConvert.SerializeObject(message));
    }

    private ResponseMessageDto<Permission> BuildMessage(int statusCode, Permission permission, string message)
    {
        ResponseMessageDto<Permission> response = new ResponseMessageDto<Permission>();
        response.IsSuccess = statusCode != StatusCodes.Status200OK ? false : true;
        response.Data = permission;
        response.StatusCode = statusCode;
        response.ErrorMessage = message;
        return response;
    }

}
