using FluentValidation;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.Trip;
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
            .WithErrorCode(ETripErrorCodes.TripDtoNull.ToString())
            .WithMessage("Trip Dto canno't be null");

        #endregion


        #region Name

        RuleFor(trip => trip.Name)
            //Check if the name is not empty
            .NotEmpty()
            .WithErrorCode(ETripErrorCodes.TripNameNotNull.ToString())
            .WithMessage("Trip Name is required")
            //Check if the name is not longer than 50 characters
            .MaximumLength(50)
            .WithErrorCode(ETripErrorCodes.TripNameMaxLength.ToString())
            .WithMessage("Name cannot be longer than 50 characters")
            //Check if the name is not shorter than 3 characters
            .MinimumLength(3)
            .WithErrorCode(ETripErrorCodes.TripNameMinLength.ToString())
            .WithMessage("Name cannot be shorter than 3 characters")
            //Check if the name is unique in trip list of the user
            .MustAsync(async (dto, name, cancellationToken) =>
            {
                List<Trip>? tripList = await tripPlatform.GetTripListByUserIdAsync(dto.UserId);
                return tripList.All(trip => trip.Name.ToUpper() != name.ToUpper());
            })
            .WithErrorCode(ETripErrorCodes.TripNameUniqueByUser.ToString())
            .WithMessage((trip) => $"The account with id: {trip.UserId}. Allready has a trip with name : {trip.Name}");


        #endregion

        #region Description

        //Check if the description is not longer than 500 characters
        RuleFor(trip => trip.Description)
            .MaximumLength(500)
            .WithErrorCode(ETripErrorCodes.TripDescriptionMaxLength.ToString())
            .WithMessage("Description cannot be longer than 500 characters");

        #endregion

        #region StartDate

        RuleFor(trip => trip.StartDate)
            //Check if the start date is not empty
            .NotEmpty()
            .WithErrorCode(ETripErrorCodes.TripStartDateNotNull.ToString())
            .WithMessage("StartDate is required")
            //Check the date type
            .Must(startDate => startDate is DateOnly)
            .WithErrorCode(ETripErrorCodes.TripStartDateNotDateOnly.ToString())
            .WithMessage("StartDate must be of type DateOnly")
            //Check if the start date is not in the past
            .Must(startDate =>
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                return startDate >= today;
            })
            .WithErrorCode(ETripErrorCodes.TripStartDateNotInPast.ToString())
            .WithMessage("StartDate cannot be in the past");

        #endregion

        #region EndDate

        RuleFor(trip => trip.EndDate)
            //Check the end date type
            .Must(endDate => endDate is DateOnly)
            .Unless(trip => trip.EndDate is null)
            .WithErrorCode(ETripErrorCodes.TripEndDateNotDateOnly.ToString())
            .WithMessage("EndDate must be of type DateOnly")
            //Check if the end date is not in the past
            .Must(endDate =>
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                return endDate >= today;
            })
            .Unless(trip => trip.EndDate is null)
            .WithErrorCode(ETripErrorCodes.TripEndDateNotInPast.ToString())
            .WithMessage("EndDate cannot be in the past")
            //Check if the end date is after the start date
            .Must((trip, endDate) => endDate >= trip.StartDate)
            .Unless(trip => trip.EndDate is null)
            .WithErrorCode(ETripErrorCodes.TripEndDateNotBeforStartDate.ToString())
            .WithMessage("EndDate cannot be before StartDate");

        #endregion
    }
}
