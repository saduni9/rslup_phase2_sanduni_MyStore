// FlightDetails.cs
public class FlightDetails
{
	public string Origin { get; set; }
	public string Destination { get; set; }
	public DateTime DepartureDate { get; set; }
	public DateTime? ReturnDate { get; set; }
}

// FlightController.cs
[ApiController]
[Route("api/[controller]")]
public class FlightController : ControllerBase
{
	[HttpPost("book")]
	public IActionResult BookFlight([FromBody] FlightDetails flightDetails)
	{
		// Perform booking logic here
		// For simplicity, just return a success message
		var bookingConfirmation = $"Flight booked successfully from {flightDetails.Origin} to {flightDetails.Destination} on {flightDetails.DepartureDate}";

		return Ok(bookingConfirmation);
	}
}

// Startup.cs
public class Startup
{
	// ... (other configurations)

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddControllers();
	}

	public void Configure(IApplicationBuilder app, IHostingEnvironment env)
	{
		// ... (other configurations)

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
		});
	}
}

