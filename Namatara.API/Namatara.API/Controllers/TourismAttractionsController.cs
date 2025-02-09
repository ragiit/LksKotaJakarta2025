using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Namatara.API.Models;

namespace Namatara.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourismAttractionsController(ApplicationDbContext context) : ControllerBase
    {
        // GET: api/TourismAttractions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<_ApiResponse<TourismAttraction>>> GetTourismAttraction(Guid id)
        {
            var attraction = await context.TourismAttractions
                .Include(x => x.Ratings)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (attraction == null)
            {
                return NotFound(new _ApiResponse<object>
                (
                    statusCode: StatusCodes.Status404NotFound,
                    message: "Id is not found."
                ));
            }

            // Hitung rata-rata rating
            var averageRating = attraction.Ratings.Count != 0
                ? attraction.Ratings.Average(r => r.Rating)
                : 0;

            return Ok(new _ApiResponse<object>
            (
                data: new
                {
                    attraction.Id,
                    attraction.CategoryId,
                    attraction.Price,
                    attraction.Name,
                    attraction.Description,
                    attraction.Location,
                    attraction.OpeningHours,
                    Rating = averageRating,
                    attraction.ImageUrl
                }
            ));
        }

        [HttpPost("ratings")]
        [Authorize]
        public async Task<ActionResult<_ApiResponse<object>>> AddRating([FromBody] TourismAttractionUserRating request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new _ApiResponse<object>(
                    statusCode: StatusCodes.Status401Unauthorized,
                    message: "Invalid user authentication."
                ));
            }

            // Check if the attraction exists
            var attraction = await context.TourismAttractions.FindAsync(request.TourismAttractionId);
            if (attraction == null)
            {
                return NotFound(new _ApiResponse<object>(
                    statusCode: StatusCodes.Status404NotFound,
                    message: "Tourism attraction not found."
                ));
            }

            // Validate if the user has already rated this attraction
            var existingRating = await context.TourismAttractionRatings
                .FirstOrDefaultAsync(r => r.TourismAttractionId == request.TourismAttractionId && r.UserId == userId);

            if (existingRating != null)
            {
                existingRating.Rating = request.Rating;
                await context.SaveChangesAsync();

                return Ok(new _ApiResponse<object>(
                    message: "Rating updated successfully."
                ));
                //return BadRequest(new _ApiResponse<object>(
                //    statusCode: StatusCodes.Status400BadRequest,
                //    message: "You have already rated this attraction."
                //));
            }

            // Add the new rating
            var newRating = new TourismAttractionRating
            {
                Id = Guid.NewGuid(),
                TourismAttractionId = request.TourismAttractionId,
                UserId = userId,
                Rating = request.Rating,
                Review = request.Review
            };

            context.TourismAttractionRatings.Add(newRating);
            await context.SaveChangesAsync();

            return Ok(new _ApiResponse<object>(
                message: "Rating added successfully."
            ));
        }

        [HttpPost("book-ticket")]
        [Authorize]
        public async Task<IActionResult> BookTicket([FromBody] TicketBookingRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new _ApiResponse<object>(
                    statusCode: StatusCodes.Status401Unauthorized,
                    message: "Invalid user authentication."
                ));
            }

            bool checkBookToday = await context.TicketBookings.AnyAsync(
                x => x.UserId == userId && x.BookingDate.Date == DateTime.Now.Date &&
                     x.TourismAttractionId == request.TourismAttractionId
            );

            if (checkBookToday)
            {
                return BadRequest(new _ApiResponse<object>(
                    statusCode: StatusCodes.Status400BadRequest,
                    message: "You have already booked a ticket today."
                ));
            }

            if (request.BookingDate.Date < DateTime.Now.Date)
            {
                return BadRequest(new _ApiResponse<object>(
                    statusCode: StatusCodes.Status400BadRequest,
                    message: "Booking date must be today or later."
                ));
            }

            if (request.BookingExpiredDate.Date < DateTime.Now.Date)
            {
                return BadRequest(new _ApiResponse<object>(
                    statusCode: StatusCodes.Status400BadRequest,
                    message: "Booking expired date must be today or later."
                ));
            }

            if (request.BookingExpiredDate.Date < request.BookingDate.Date)
            {
                return BadRequest(new _ApiResponse<object>(
                    statusCode: StatusCodes.Status400BadRequest,
                    message: "Booking expired date must be later than booking date."
                ));
            }

            var tourism =
                await context.TourismAttractions.FirstOrDefaultAsync(x => x.Id == request.TourismAttractionId);

            if (tourism == null)
                return NotFound(new _ApiResponse<object>(
                    statusCode: StatusCodes.Status404NotFound,
                    message: "Tourism attraction not found."
                ));

            var totalPrice = request.NumberOfTickets * tourism.Price;

            if (totalPrice != 0 && request.InputPrice < totalPrice)
            {
                return BadRequest(new _ApiResponse<object>(
                    statusCode: StatusCodes.Status400BadRequest,
                    message:
                    $"Input price is less than the total price. Please check again. Total price: {totalPrice} Input price: {request.InputPrice}"
                ));
            }

            var booking = new TicketBooking
            {
                Id = Guid.NewGuid(),
                Code = GenerateBookingCode(6),
                TourismAttractionId = request.TourismAttractionId,
                UserId = userId,
                NumberOfTickets = request.NumberOfTickets,
                InputPrice = request.InputPrice,
                Price = totalPrice == 0 ? 0 : tourism.Price,
                TotalPrice = totalPrice == 0 ? 0 : totalPrice,
                BookingDate = request.BookingDate,
                BookingExpiredDate = request.BookingExpiredDate
            };
            await context.TicketBookings.AddAsync(booking);
            await context.SaveChangesAsync();

            return Ok(new _ApiResponse<object>(
                message: "Ticket booked successfully."
            ));
        }

        private static string GenerateBookingCode(int length = 6)
        {
            const string validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var code = new char[length];

            for (int i = 0; i < length; i++)
            {
                code[i] = validCharacters[random.Next(validCharacters.Length)];
            }

            return new string(code);
        }
    }
}