using System.Security.Claims;

public class CheckCookiesSession
{
    private readonly RequestDelegate _next;

    public CheckCookiesSession(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var username = context.User.FindFirst(ClaimTypes.Name)?.Value;
            // Khi nào cần check thì mở ra
            // Console.WriteLine($"User đang đăng nhập: {username}");
        }

        await _next(context);
    }
}
