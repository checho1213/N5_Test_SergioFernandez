﻿using MediatR;
using Microsoft.AspNetCore.Http;
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

public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, ResponseMessageDto<Permission>>
{
    #region privateMembers 
    private readonly IUnitofWork _unitofWork;
    private readonly IKafkaRepository _kafkaRepository;
    private readonly string permissionEventTopic;
    private readonly IPermissionDomainServices _permissionDomainServices;
    #endregion

    public CreatePermissionCommandHandler(IUnitofWork unitOfWork, IKafkaRepository kafkaRepository, IOptions<InfraestructureSettings> infraSettings
        , IPermissionDomainServices permissionDomainServices)
    {
        _unitofWork = unitOfWork;
        _kafkaRepository = kafkaRepository;
        permissionEventTopic = infraSettings.Value.KafkaSettings.Topics.PermissionEvent;
        _permissionDomainServices = permissionDomainServices;
    }

    public async Task<ResponseMessageDto<Permission>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        ResponseMessageDto<Permission> response = new ResponseMessageDto<Permission>();
        int statusCode = StatusCodes.Status500InternalServerError;
        try
        {
            var resultValidation = _permissionDomainServices.ValidateDatePermission(request.PermissionDate);
            if (!resultValidation)
            {
                statusCode = StatusCodes.Status400BadRequest;
                throw new DomainExceptions("La fecha no valida.", new Exception("La fecha del permiso no puede ser inferior a la fecha actual."));
            }
            Domain.Entities.Permission permiso = new Domain.Entities.Permission();
            permiso.EmployeeName = request.EmployeeName;
            permiso.LastName = request.EmployeeSurname;
            permiso.Date = DateOnly.FromDateTime(request.PermissionDate);
            permiso.PemissionTypeId = request.PermissionType;
            _unitofWork.PermisosRepository.Add(permiso);
            await SendMessageToPermissionEventsTopic(permiso);
            return BuildMessage(StatusCodes.Status201Created, permiso, "Permiso creado con exito");
        }
        catch (Exception ex)
        {
            return BuildMessage(statusCode, null, ex.InnerException.Message);
        }
    }

    private async Task SendMessageToPermissionEventsTopic(Permission permission)
    {
        PermissionEnventDto message = new PermissionEnventDto();
        message.Operation = "Create";
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
