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
            .NotEmpty()
            .WithErrorCode(nameof(ETripErrorCodes.TripDtoNull))
            .WithMessage("Trip Dto canno't be null");

        #endregion

        #region UserId

        //Check if the UserId is not empty
        RuleFor(trip => trip.UserId)
            .NotEmpty()
            .WithErrorCode(nameof(EMapUserErrorCodes.UserIdNotNull))
            .WithMessage("UserId is required")
        //Check if the user exists with userManager of entity framework
        .MustAsync(async (trip, userId, cancellationToken) =>
        {
            MapUser? user = await userManager.FindByIdAsync(userId.ToString());
            return user is not null; 
        })
        .WithErrorCode(nameof(EMapUserErrorCodes.UserNotFoundById))
        .WithMessage("User not found");

        #endregion

        #region Name

        RuleFor(trip => trip.Name)
            //Check if the name is not empty
            .NotEmpty()
            .WithErrorCode(nameof(ETripErrorCodes.TripNameNotNull))
            .WithMessage("Trip Name is required")
            //Check if the name is not longer than 50 characters
            .MaximumLength(50)
            .WithErrorCode(nameof(ETripErrorCodes.TripNameMaxLength))
            .WithMessage("Name cannot be longer than 50 characters")
            //Check if the name is not shorter than 3 characters
            .MinimumLength(3)
            .WithErrorCode(nameof(ETripErrorCodes.TripNameMinLength))
            .WithMessage("Name cannot be shorter than 3 characters")
            //Check if the name is unique in trip list of the user
            .MustAsync(async (dto, name, cancellationToken) =>
            {
                List<Trip>? tripList = await tripPlatform.GetTripListByUserIdAsync(dto.UserId);
                return tripList.All(trip => trip.Name.ToUpper() != name.ToUpper());
            })
            .WithErrorCode(nameof(ETripErrorCodes.TripNameUniqueByUser))
            .WithMessage((trip) => $"The account with id: {trip.UserId}. Allready has a trip with name : {trip.Name}");


        #endregion

        #region Description

        //Check if the description is not longer than 500 characters
        RuleFor(trip => trip.Description)
            .MaximumLength(500)
            .WithErrorCode(nameof(ETripErrorCodes.TripDescriptionMaxLength))
            .WithMessage("Description cannot be longer than 500 characters");

        #endregion

        #region StartDate

        RuleFor(trip => trip.StartDate)
            //Check if the start date is not empty
            .NotEmpty()
            .WithErrorCode(nameof(ETripErrorCodes.TripStartDateNotNull))
            .WithMessage("StartDate is required")
            //Check the date type
            .Must(startDate => startDate is DateOnly)
            .WithErrorCode(nameof(ETripErrorCodes.TripStartDateNotDateOnly))
            .WithMessage("StartDate must be of type DateOnly")
            //Check if the start date is not in the past
            .Must(startDate =>
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                return startDate >= today;
            })
            .WithErrorCode(nameof(ETripErrorCodes.TripStartDateNotInPast))
            .WithMessage("StartDate cannot be in the past");

        #endregion

        #region EndDate

        RuleFor(trip => trip.EndDate)
            //Check if the end date is not empty
            .NotEmpty()
            .WithErrorCode(nameof(ETripErrorCodes.TripEndDateNotNull))
            .WithMessage("EndDate is required")
            //Check the end date type
            .Must(endDate => endDate is DateOnly)
            .WithErrorCode(nameof(ETripErrorCodes.TripEndDateNotDateOnly))
            .WithMessage("EndDate must be of type DateOnly")
            //Check if the end date is not in the past
            .Must(endDate =>
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                return endDate >= today;
            })
            .WithErrorCode(nameof(ETripErrorCodes.TripEndDateNotInPast))
            .WithMessage("EndDate cannot be in the past")
            //Check if the end date is after the start date
            .Must((trip, endDate) => endDate >= trip.StartDate)
            .WithErrorCode(nameof(ETripErrorCodes.TripEndDateNotBeforStartDate))
            .WithMessage("EndDate cannot be before StartDate");

        #endregion
    }
}
