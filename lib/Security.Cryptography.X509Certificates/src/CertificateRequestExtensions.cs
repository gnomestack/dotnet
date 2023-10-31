using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GnomeStack.Security.Cryptography.X509Certificates;

public static class CertificateRequestExtensions
{
    public static string ExportToPem(this CertificateRequest request, X509SignatureGenerator? generator = null)
    {
        byte[] pkcs10 = generator is null ?
            request.CreateSigningRequest() :
            request.CreateSigningRequest(generator);

        var builder = new StringBuilder();

        builder.AppendLine("-----BEGIN CERTIFICATE REQUEST-----");

        string base64 = Convert.ToBase64String(pkcs10);

        int offset = 0;
        const int lineLength = 64;

        while (offset < base64.Length)
        {
            int lineEnd = Math.Min(offset + lineLength, base64.Length);
            builder.Append(base64, offset, lineEnd - offset).AppendLine();
            offset = lineEnd;
        }

        builder.AppendLine("-----END CERTIFICATE REQUEST-----");
        var result = builder.ToString();
        builder.Clear();
        return result;
    }
}