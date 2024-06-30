using AuthServices.Domain.Models;

namespace AuthServices.API.Helpers
{
    public class CookiesHelper
    {
        
        public HttpResponse SetRefreshTokenInCookie(HttpResponse httpResponse, string refreshToken, DateTime expires)
        {
            httpResponse = RemoveRefreshFromCookie(httpResponse);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            httpResponse.Cookies.Append(CookiesTokens.RefreshTokenName, refreshToken, cookieOptions);
            return httpResponse;
        }

        public HttpResponse SetAccessToken(HttpResponse httpResponse, string token, DateTime expires)
        {
            httpResponse = RemoveAccessFromCookie(httpResponse);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            httpResponse.Cookies.Append(CookiesTokens.AccessTokenName, token, cookieOptions);
            return httpResponse;
        }

        public HttpResponse RemoveTokensFromCookie(HttpResponse httpResponse)
        {
            httpResponse = RemoveTokenFromCookie(httpResponse, CookiesTokens.AccessTokenName);
            httpResponse = RemoveTokenFromCookie(httpResponse, CookiesTokens.RefreshTokenName);
            return httpResponse;
        }

        private HttpResponse RemoveAccessFromCookie(HttpResponse httpResponse) => RemoveTokenFromCookie(httpResponse, CookiesTokens.AccessTokenName);

        private HttpResponse RemoveRefreshFromCookie(HttpResponse httpResponse) => RemoveTokenFromCookie(httpResponse, CookiesTokens.RefreshTokenName);

        private HttpResponse RemoveTokenFromCookie(HttpResponse httpResponse, string tokenName)
        {
            if (httpResponse.HttpContext.Request.Cookies.TryGetValue(tokenName, out string _))
            {
                httpResponse.Cookies.Delete(tokenName);
            }
            return httpResponse;
        }

    }
}
