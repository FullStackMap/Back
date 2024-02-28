using FluentValidation;
using Map.API.Extension;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.TripDto;
using Map.Platform.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Map.API.Validator.TripValidator;

public class UpdateTripValidator : AbstractValidator<UpdateTripDto>
{
    public UpdateTripValidator(ITripPlatform tripPlatform, UserManager<MapUser> userManager)
    {
        if (tripPlatform is null) throw new ArgumentNullException(nameof(tripPlatform));
        if (userManager is null) throw new ArgumentNullException(nameof(userManager));

        #region Dto

        //Check if Dto is not null 
        RuleFor(dto => dto)
            .NotEmpty()
            .WithErrorCode(ETripErrorCodes.TripDtoNull.ToStringValue())
            .WithMessage("Update Trip Dto canno't be null");

        #endregion


        #region UserId

        //Check if the UserId is not empty
        RuleFor(trip => trip.UserId)
            .NotEmpty()
            .WithErrorCode(EMapUserErrorCodes.UserIdNotNull.ToStringValue())
            .WithMessage("UserId is required")
        //Check if the user exists with userManager of entity framework
        .MustAsync(async (trip, userId, cancellationToken) =>
        {
            MapUser? user = await userManager.FindByIdAsync(userId.ToString());
            return user is not null;
        })
        .WithErrorCode(EMapUserErrorCodes.UserNotFoundById.ToStringValue())
        .WithMessage("User not found");

        #endregion

        #region Name

        RuleFor(trip => trip.Name)
            //Check if the name is not empty
            .NotEmpty()
            .WithErrorCode(ETripErrorCodes.TripNameNotNull.ToStringValue())
            .WithMessage("Trip Name is required")
            //Check if the name is not longer than 50 characters
            .MaximumLength(50)
            .WithErrorCode(ETripErrorCodes.TripNameMaxLength.ToStringValue())
            .WithMessage("Name cannot be longer than 50 characters")
            //Check if the name is not shorter than 3 characters
            .MinimumLength(3)
            .WithErrorCode(ETripErrorCodes.TripNameMinLength.ToStringValue())
            .WithMessage("Name cannot be shorter than 3 characters");

        #endregion

        #region Description

        //Check if the description is not longer than 500 characters
        RuleFor(trip => trip.Description)
            .MaximumLength(500)
            .WithErrorCode(ETripErrorCodes.TripDescriptionMaxLength.ToStringValue())
            .WithMessage("Description cannot be longer than 500 characters");

        #endregion

        #region StartDate

        RuleFor(trip => trip.StartDate)
            //Check if the start date is not empty
            .NotEmpty()
            .WithErrorCode(ETripErrorCodes.TripStartDateNotNull.ToStringValue())
            .WithMessage("StartDate is required")
            //Check the date type
            .Must(startDate => startDate is DateOnly)
            .WithErrorCode(ETripErrorCodes.TripStartDateNotDateOnly.ToStringValue())
            .WithMessage("StartDate must be of type DateOnly");

        #endregion

        #region EndDate

        RuleFor(trip => trip.EndDate)
            //Check if the end date is not empty
            .NotEmpty()
            .WithErrorCode(ETripErrorCodes.TripEndDateNotNull.ToStringValue())
            .WithMessage("EndDate is required")
            //Check the end date type
            .Must(endDate => endDate is DateOnly)
            .WithErrorCode(ETripErrorCodes.TripEndDateNotDateOnly.ToStringValue())
            .WithMessage("EndDate must be of type DateOnly")
            //Check if the end date is not in the past
            .Must(endDate =>
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                return endDate >= today;
            })
            .WithErrorCode(ETripErrorCodes.TripEndDateNotInPast.ToStringValue())
            .WithMessage("EndDate cannot be in the past")
            //Check if the end date is after the start date
            .Must((trip, endDate) => endDate >= trip.StartDate)
            .WithErrorCode(ETripErrorCodes.TripEndDateNotBeforStartDate.ToStringValue())
            .WithMessage("EndDate cannot be before StartDate");

        #endregion
    }
}