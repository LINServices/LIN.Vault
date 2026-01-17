#if WINDOWS
using Windows.Security.Credentials.UI;
#else
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
#endif

namespace LIN.Authenticator.Services;

public class BiometricService : IBiometricService
{
    public async Task<bool> IsAvailableAsync()
    {
#if WINDOWS
        try
        {
            var availability = await UserConsentVerifier.CheckAvailabilityAsync();
            return availability == UserConsentVerifierAvailability.Available;
        }
        catch
        {
            return false;
        }
#else
        try
        {
            return await CrossFingerprint.Current.IsAvailableAsync();
        }
        catch
        {
            return false;
        }
#endif
    }

    public async Task<bool> AuthenticateAsync(string title, string reason)
    {
#if WINDOWS
        try
        {
            var result = await UserConsentVerifier.RequestVerificationAsync(reason);
            return result == UserConsentVerificationResult.Verified;
        }
        catch
        {
            return false;
        }
#else
        try
        {
            var request = new AuthenticationRequestConfiguration(title, reason);
            var result = await CrossFingerprint.Current.AuthenticateAsync(request);
            return result.Authenticated;
        }
        catch (Exception)
        {
            return false;
        }
#endif
    }
}
