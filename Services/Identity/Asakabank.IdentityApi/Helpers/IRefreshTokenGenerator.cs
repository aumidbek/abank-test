namespace Asakabank.IdentityApi.Helpers {
    public interface IRefreshTokenGenerator {
        string GenerateToken();
    }
}