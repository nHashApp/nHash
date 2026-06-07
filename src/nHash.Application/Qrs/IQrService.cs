namespace nHash.Application.Qrs;

public interface IQrService
{
    byte[] GeneratePng(string text, QrErrorCorrectionLevel eccLevel = QrErrorCorrectionLevel.Medium, int pixelSize = 20);
    string GenerateSvg(string text, QrErrorCorrectionLevel eccLevel = QrErrorCorrectionLevel.Medium);
    string GenerateAscii(string text, QrErrorCorrectionLevel eccLevel = QrErrorCorrectionLevel.Medium, string darkColorString = "██", string whiteSpaceString = "  ");
}
