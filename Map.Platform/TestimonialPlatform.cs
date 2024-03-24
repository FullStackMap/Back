using Map.Domain.Entities;
using Map.EFCore.Interfaces;
using Map.Platform.Interfaces;

namespace Map.Platform;
internal class TestimonialPlatform : ITestimonialPlatform
{
    #region props
    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Ctor
    public TestimonialPlatform(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    #endregion

    /// <inheritdoc/>
    public async Task AddTestimonialAsync(MapUser user, Testimonial testimonial)
    {
        await _unitOfWork.Testimonial.AddTestimonial(user, testimonial);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public Task DeleteTestimonialAsync(Testimonial testimonial)
    {
        _unitOfWork.Testimonial.Remove(testimonial);
        return _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public IQueryable<Testimonial> GetAllTestimonials() => _unitOfWork.Testimonial.GetAll();

    /// <inheritdoc/>
    public async Task<Testimonial?> GetByTestimonialIdAsync(int testimonialId) => await _unitOfWork.Testimonial.GetByIdAsync(testimonialId);
}
