using System;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

// ReSharper disable InconsistentNaming
// ECDsa is how .NET names the cipher.
namespace GnomeStack.Security.Cryptography.X509Certificates;

public interface ICertificateRequestBuilder
{
    ICertificateRequestBuilder AsCertificateAuthority(bool isCertificateAuthority = true);

    CertificateRequest Build();

    X509Certificate2 BuildCertificate();

    ICertificateRequestBuilder Reset();

    ICertificateRequestBuilder WithDnsNames(params string[]? dnsNames);

    ICertificateRequestBuilder WithEmails(params string[]? emails);

    ICertificateRequestBuilder WithIpAddresses(params IPAddress[]? ipAddresses);

    ICertificateRequestBuilder WithUserPrincipalNames(params string[]? userPrincipalNames);

    ICertificateRequestBuilder WithRsa(int keySize, RSASignaturePadding? padding = null, bool force = false);

    ICertificateRequestBuilder WithECDsa(int keySize);

    ICertificateRequestBuilder WithEnhancedKeyUsages(params Oid[] enhancedKeyUsages);

    ICertificateRequestBuilder WithEnhancedKeyUsages(params EnhancedKeyUsageOids[] enhancedKeyUsages);

    ICertificateRequestBuilder WithKeyUsage(X509KeyUsageFlags flags);

    ICertificateRequestBuilder WithSigningAlgorithm(HashAlgorithmName name);

    ICertificateRequestBuilder WithExtensions(params X509Extension[] extensions);

    ICertificateRequestBuilder WithPathLengthConstraint(int pathLengthConstraint, bool critical);

    ICertificateRequestBuilder WithIssuer(X509Certificate2 issuer);

    ICertificateRequestBuilder WithSubject(string subject);

    ICertificateRequestBuilder WithStartDate(DateTimeOffset startAt);

    ICertificateRequestBuilder WithEndDate(DateTimeOffset endAt);

    ICertificateRequestBuilder WithDateRange(DateTimeOffset startAt, DateTimeOffset endAt);

    ICertificateRequestBuilder WithSerialNumber(byte[] serialNumber);

    ICertificateRequestBuilder WithSerialNumber(int serialNumber);

    ICertificateRequestBuilder WithSerialNumber(long serialNumber);

    ICertificateRequestBuilder WithUrls(params Uri[]? uris);
}