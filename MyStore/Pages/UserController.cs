using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private List<User> users = new List<User>
    {
        new User { Id = 1, Username = "user1", Password = "password1" },
        new User { Id = 2, Username = "user2", Password = "password2" }
    };

    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] User newUser)
    {
        users.Add(newUser);
        return CreatedAtAction("GetUser", new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound();

        user.Username = updatedUser.Username;
        user.Password = updatedUser.Password;

        return Ok(user);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound();

        users.Remove(user);
        return NoContent();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User loginUser)
    {
        var user = users.FirstOrDefault(u => u.Username == loginUser.Username && u.Password == loginUser.Password);
        if (user == null)
            return Unauthorized();

        var token = GenerateToken(user);
        return Ok(new { User = user, Token = token });
    }

    [Authorize] // This attribute requires authentication for the following actions
    [HttpGet("profile")]
    public IActionResult UserProfile()
    {
        // This is a protected endpoint that requires authentication
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int.TryParse(userId, out int id);

        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    private string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "YourIssuer",
            audience: "YourAudience",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
