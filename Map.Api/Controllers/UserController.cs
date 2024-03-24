using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Map.API.MailTemplate;
using Map.Domain.Models.EmailDto;
using Map.Domain.Models.User;
using Map.Platform.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Map.API.Controllers.Models.HttpError;

namespace Map.API.Controllers;

[ApiController]
[ApiVersion(ApiControllerVersions.V1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    #region Props

    private readonly IValidator<SupportContactMailDto> _supportContactMailValidator;
    private readonly IMailPlatform _mailPlatform;


    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public UserController(IMapper mapper,
                          IValidator<SupportContactMailDto> contactMailValidator,
                          IMailPlatform mailPlatform)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _supportContactMailValidator = contactMailValidator ?? throw new ArgumentNullException(nameof(contactMailValidator));
        _mailPlatform = mailPlatform ?? throw new ArgumentNullException(nameof(mailPlatform));
    }

    #endregion


    [HttpPost]
    [Route("SupportContact")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ICollection<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ContactMailAsync([FromBody] SupportContactMailDto supportContactMailDto)
    {
        ValidationResult validationResult = _supportContactMailValidator.Validate(supportContactMailDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        string contactMailTemplate = _mailPlatform.GetTemplate(TemplatesName.SupportContactMail);

        MailDto mailDto = _mapper.Map<SupportContactMailDto, MailDto>(supportContactMailDto);
        mailDto.Body = contactMailTemplate.Replace("[Name]", supportContactMailDto.Name)
                                          .Replace("[Email]", supportContactMailDto.Email)
                                          .Replace("[Subject]", supportContactMailDto.Subject)
                                          .Replace("[Body]", supportContactMailDto.Body);

        mailDto.Subject = $"Demande de support : {mailDto.Name}";

        mailDto.SenderName = supportContactMailDto.Name;
        mailDto.SenderMail = supportContactMailDto.Email;
        mailDto.IsMailToUser = false;

        await _mailPlatform.SendMailAsync(mailDto);

        return Ok();
    }
}