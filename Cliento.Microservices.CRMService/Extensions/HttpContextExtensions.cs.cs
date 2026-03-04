namespace Cliento.Microservices.CRMService.Extensions
{
    public static class HttpContextExtensions
    {
        public static bool TryGetUserId(this HttpContext ctx, out Guid userId)
        {
            userId = Guid.Empty;
            if (!ctx.Request.Headers.TryGetValue("X-User-Id", out var vals)) return false;
            if (Guid.TryParse(vals.FirstOrDefault(), out userId)) return true;
            return false;
        }
    }
}
