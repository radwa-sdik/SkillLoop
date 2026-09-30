namespace SkillLoop.Infrasturcture.Security
{
    public class JwtOptions
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int AccessTokenMinutes { get; set; } = 30;
        public int RefreshTokenDays { get; set; } = 30;
    }
}
