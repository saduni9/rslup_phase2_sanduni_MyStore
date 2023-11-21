using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private static Dictionary<string, string> resetTokens = new Dictionary<string, string>();
    private static Dictionary<string, string> userPasswords = new Dictionary<string, string>();

    [HttpPost("forgot-password")]
    public IActionResult ForgotPassword([FromBody] ForgotPasswordRequest model)
    {
        // Check if the email exists in your user database (this is a simplified example)
        string userEmail = GetUserEmail(model.Email);

        if (userEmail != null)
        {
            // Generate a reset token (for simplicity, using a simple GUID here)
            string resetToken = Guid.NewGuid().ToString();

            // Store the reset token for later verification
            resetTokens[model.Email] = resetToken;

            // Send a password reset email (for simplicity, just logging here)
            Console.WriteLine($"Sending password reset email to {model.Email}. Reset Token: {resetToken}");

            return Ok(new { Message = "Password reset email sent successfully." });
        }
        else
        {
            return BadRequest(new { Message = "Email not found." });
        }
    }

    [HttpPost("reset-password")]
    public IActionResult ResetPassword([FromBody] ResetPasswordRequest model)
    {
        // Check if the email and reset token match
        if (resetTokens.TryGetValue(model.Email, out string storedToken) && storedToken == model.ResetToken)
        {
            // Update the password (this is a simplified example)
            userPasswords[model.Email] = model.NewPassword;

            // Clear the reset token after using it
            resetTokens.Remove(model.Email);

            return Ok(new { Message = "Password reset successfully." });
        }
        else
        {
            return BadRequest(new { Message = "Invalid reset token or email." });
        }
    }

    // Helper method to simulate checking if the email exists in your user database
    private string GetUserEmail(string email)
    {
        // This is a simplified example; in a real application, you would query your user database
        // to check if the email exists and return the user's actual email.
        // For simplicity, we assume a predefined user here.
        return email == "user@example.com" ? email : null;
    }
}

public class ForgotPasswordRequest
{
    public string Email { get; set; }
}

public class ResetPasswordRequest
{
    public string Email { get; set; }
    public string ResetToken { get; set; }
    public string NewPassword { get; set; }
}
