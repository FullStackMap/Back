using FluentValidation;
using Map.API.Models.TripDto;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Platform.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Map.API.Validator.TripValidator;

public class AddTripValidator : AbstractValidator<AddTripDto>
{
    public AddTripValidator(ITripPlatform tripPlatform, UserManager<MapUser> userManager)
    {
        if (tripPlatform is null) throw new ArgumentNullException(nameof(tripPlatform));
        if (userManager is null) throw new ArgumentNullException(nameof(userManager));

        #region Dto

        //Check if the dto is null
        RuleFor(dto => dto)
            .NotNull()
            .WithErrorCode(nameof(ETripErrorCodes.TripDtoNull))
            .WithMessage("Trip Dto canno't be null");

        #endregion

        #region UserId

        //Check if the UserId is not empty
        RuleFor(trip => trip.UserId)
            .NotEmpty()
            .WithMessage("UserId is required")
            .WithErrorCode(nameof(EMapUserErrorCodes.UserIdNotNull))
        //Check if the user exists with userManager of entity framework
        .MustAsync(async (trip, userId, cancellationToken) =>
        {
            MapUser? user = await userManager.FindByIdAsync(userId.ToString());
            return user is not null;
        })
        .WithMessage("User not found")
        .WithErrorCode(nameof(EMapUserErrorCodes.UserNotFoundById));

        #endregion

        #region Name

        RuleFor(trip => trip.Name)
            //Check if the name is not empty
            .NotEmpty()
            .WithMessage("Trip Name is required")
            .WithErrorCode(nameof(ETripErrorCodes.TripNameNotNull))
            .MaximumLength(50)
            //Check if the name is not longer than 50 characters
            .WithMessage("Name cannot be longer than 50 characters")
            .WithErrorCode(nameof(ETripErrorCodes.TripNameMaxLength))
            //Check if the name is not shorter than 3 characters
            .MinimumLength(3)
            .WithMessage("Name cannot be shorter than 3 characters")
            .WithErrorCode(nameof(ETripErrorCodes.TripNameMinLength))
            //Check if the name is unique in trip list of the user
            .MustAsync(async (dto, name, cancellationToken) =>
            {
                List<Trip>? tripList = await tripPlatform.GetTripListByUserIdAsync(dto.UserId);
                return tripList.All(trip => trip.Name.ToUpper() != name.ToUpper());
            })
            .WithMessage((trip) => $"The account with id: {trip.UserId}. Allready have trip with name : {trip.Name}")
            .WithErrorCode(nameof(ETripErrorCodes.TripNameUniqueByUser));


        #endregion

        #region Description

        //Check if the description is not longer than 500 characters
        RuleFor(trip => trip.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot be longer than 500 characters")
            .WithErrorCode(nameof(ETripErrorCodes.TripDescriptionMaxLength));

        #endregion

        #region StartDate

        RuleFor(trip => trip.StartDate)
            //Check if the start date is not empty
            .NotEmpty()
            .WithMessage("StartDate is required")
            .WithErrorCode(nameof(ETripErrorCodes.TripStartDateNotNull))
            //Check if the start date is not in the past
            .Must(startDate => startDate is DateOnly)
            .WithMessage("StartDate must be of type DateOnly")
            .WithErrorCode(nameof(ETripErrorCodes.TripStartDateNotDateOnly))
            //Check the date type
            .Must(startDate =>
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                return startDate >= today;
            })
            .WithMessage("StartDate cannot be in the past")
            .WithErrorCode(nameof(ETripErrorCodes.TripStartDateNotInPast));

        #endregion

        #region EndDate

        RuleFor(trip => trip.EndDate)
            //Check if the end date is not empty
            .NotEmpty()
            .WithMessage("EndDate is required")
            .WithErrorCode(nameof(ETripErrorCodes.TripEndDateNotNull))
            //Check if the end date is not in the past
            .Must(endDate => endDate is DateOnly)
            .WithMessage("EndDate must be of type DateOnly")
            .WithErrorCode(nameof(ETripErrorCodes.TripEndDateNotDateOnly))
            //Check the end date type
            .Must(endDate =>
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                return endDate >= today;
            })
            .WithMessage("EndDate cannot be in the past")
            .WithErrorCode(nameof(ETripErrorCodes.TripEndDateNotInPast))
            //Check if the end date is after the start date
            .Must((trip, endDate) => endDate >= trip.StartDate)
            .WithMessage("EndDate cannot be before StartDate")
            .WithErrorCode(nameof(ETripErrorCodes.TripEndDateNotBeforStartDate))
            //Check if the end date is not the same as the start date
            .Must((trip, endDate) => endDate != trip.StartDate)
            .WithMessage("EndDate cannot be the same as StartDate")
            .WithErrorCode(nameof(ETripErrorCodes.TripEndDateNotEqualStartDate));

        #endregion
    }
}
