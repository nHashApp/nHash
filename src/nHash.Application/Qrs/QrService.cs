using QRCoder;

namespace nHash.Application.Qrs;

public class QrService : IQrService
{
    public byte[] GeneratePng(string text, QrErrorCorrectionLevel eccLevel = QrErrorCorrectionLevel.Medium, int pixelSize = 20)
    {
        var level = MapEccLevel(eccLevel);
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(text, level);
        using var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(pixelSize);
    }

    public string GenerateSvg(string text, QrErrorCorrectionLevel eccLevel = QrErrorCorrectionLevel.Medium)
    {
        var level = MapEccLevel(eccLevel);
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(text, level);
        using var qrCode = new SvgQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }

    public string GenerateAscii(string text, QrErrorCorrectionLevel eccLevel = QrErrorCorrectionLevel.Medium, string darkColorString = "██", string whiteSpaceString = "  ")
    {
        var level = MapEccLevel(eccLevel);
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(text, level);
        using var qrCode = new AsciiQRCode(qrCodeData);
        return qrCode.GetGraphic(1, darkColorString, whiteSpaceString);
    }

    private static QRCodeGenerator.ECCLevel MapEccLevel(QrErrorCorrectionLevel level)
    {
        return level switch
        {
            QrErrorCorrectionLevel.Low => QRCodeGenerator.ECCLevel.L,
            QrErrorCorrectionLevel.Medium => QRCodeGenerator.ECCLevel.M,
            QrErrorCorrectionLevel.Quartile => QRCodeGenerator.ECCLevel.Q,
            QrErrorCorrectionLevel.High => QRCodeGenerator.ECCLevel.H,
            _ => QRCodeGenerator.ECCLevel.M
        };
    }
}
