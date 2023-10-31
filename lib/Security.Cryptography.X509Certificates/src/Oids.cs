namespace GnomeStack.Security.Cryptography.X509Certificates;

/// <summary>
/// Common <see cref="System.Security.Cryptography.Oid"/> Values.
/// </summary>
// MIT: based on https://github.com/damienbod/AspNetCoreCertificates/blob/main/src/CertificateManager/OidLookup.cs
public static class Oids
{
    /// <summary>
    /// The Windows Update oid (1.3.6.1.4.1.311.76.6.1).
    /// </summary>
    public const string WindowsUpdate = "1.3.6.1.4.1.311.76.6.1";

    /// <summary>
    /// The Windows Third Party Application Component oid (1.3.6.1.4.1.311.10.3.25).
    /// </summary>
    public const string WindowsThirdPartyApplicationComponent = "1.3.6.1.4.1.311.10.3.25";

    /// <summary>
    /// The Windows System Component Verification oid (1.3.6.1.4.1.311.10.3.6).
    /// </summary>
    public const string WindowsSystemComponentVerification = "1.3.6.1.4.1.311.10.3.6";

    /// <summary>
    /// The Windows TCB Component oid (1.3.6.1.4.1.311.10.3.23).
    /// </summary>
    public const string WindowsTcbComponent = "1.3.6.1.4.1.311.10.3.23";

    /// <summary>
    /// The Windows Software Extension Verification oid (1.3.6.1.4.1.311.10.3.26).
    /// </summary>
    public const string WindowsSoftwareExtensionVerification = "1.3.6.1.4.1.311.10.3.26";

    /// <summary>
    /// The Windows Store oid (1.3.6.1.4.1.311.76.3.1).
    /// </summary>
    public const string WindowsStore = "1.3.6.1.4.1.311.76.3.1";

    /// <summary>
    /// The Windows Kits Component oid (1.3.6.1.4.1.311.10.3.20).
    /// </summary>
    public const string WindowsKitsComponent = "1.3.6.1.4.1.311.10.3.20";

    /// <summary>
    /// The Windows Hardware Driver Attested Verification (1.3.6.1.4.1.311.10.3.5.1).
    /// </summary>
    public const string WindowsHardwareDriverAttestedVerification = "1.3.6.1.4.1.311.10.3.5.1";

    /// <summary>
    /// The Windows Hardware Driver Verification oid (1.3.6.1.4.1.311.10.3.5).
    /// </summary>
    public const string WindowsHardwareDriverVerification = "1.3.6.1.4.1.311.10.3.5";

    /// <summary>
    /// The Windows Hardware Driver Extended Verification oid (1.3.6.1.4.1.311.10.3.39).
    /// </summary>
    public const string WindowsHardwareDriverExtendedVerification = "1.3.6.1.4.1.311.10.3.39";

    /// <summary>
    /// The Windows RT Verification oid (1.3.6.1.4.1.311.10.3.21).
    /// </summary>
    public const string WindowsRtVerification = "1.3.6.1.4.1.311.10.3.21";

    /// <summary>
    /// The Key Recovery oid (1.3.6.1.4.1.311.10.3.11).
    /// </summary>
    public const string KeyRecovery = "1.3.6.1.4.1.311.10.3.11";

    /// <summary>
    /// The Key Recovery Agent oid (1.3.6.1.4.1.311.21.6).
    /// </summary>
    public const string KeyRecoveryAgent = "1.3.6.1.4.1.311.21.6";

    /// <summary>
    /// The Key Pack Licenses oid (1.3.6.1.4.1.311.10.6.1).
    /// </summary>
    public const string KeyPackLicenses = "1.3.6.1.4.1.311.10.6.1";

    /// <summary>
    /// The Early Launch Anti-malware Driver oid (1.3.6.1.4.1.311.61.4.1).
    /// </summary>
    public const string EarlyLaunchAntimMalwareDriver = "1.3.6.1.4.1.311.61.4.1";

    /// <summary>
    /// The Kernel Mode Code Signing oid (1.3.6.1.4.1.311.61.1.1).
    /// </summary>
    public const string KernelModeCodeSigning = "1.3.6.1.4.1.311.61.1.1";

    /// <summary>
    /// The Attestation Identity Key Certificate oid (2.23.133.8.3).
    /// </summary>
    public const string AttestationIdentityKeyCertificate = "2.23.133.8.3";

    /// <summary>
    /// The Smart Card Logon (1.3.6.1.4.1.311.20.2.2).
    /// </summary>
    public const string SmartCardLogon = "1.3.6.1.4.1.311.20.2.2";

    /// <summary>
    /// The KDC Authentication oid (1.3.6.1.5.2.3.5).
    /// </summary>
    public const string KdcAuthentication = "1.3.6.1.5.2.3.5";

    /// <summary>
    /// The Embedded Windows System Component Verification (1.3.6.1.4.1.311.10.3.8).
    /// </summary>
    public const string EmbeddedWindowsSystemComponentVerification = "1.3.6.1.4.1.311.10.3.8";

    /// <summary>
    /// The IP Security Tunnel Termination oid (1.3.6.1.5.5.7.3.6).
    /// </summary>
    public const string IpSecurityTunnelTermination = "1.3.6.1.5.5.7.3.6";

    /// <summary>
    /// The IP Security IKE Intermediate oid (1.3.6.1.5.5.8.2.2).
    /// </summary>
    public const string IpSecurityIkeIntermediate = "1.3.6.1.5.5.8.2.2";

    /// <summary>
    /// The IP security user oid (1.3.6.1.5.5.7.3.7).
    /// </summary>
    public const string IpSecurityUser = "1.3.6.1.5.5.7.3.7";

    /// <summary>
    /// The License Server Verification oid (1.3.6.1.4.1.311.10.6.2).
    /// </summary>
    public const string LicenseServerVerification = "1.3.6.1.4.1.311.10.6.2";

    /// <summary>
    /// The Dynamic Code Generator oid (1.3.6.1.4.1.311.76.5.1).
    /// </summary>
    public const string DynamicCodeGenerator = "1.3.6.1.4.1.311.76.5.1";

    /// <summary>
    /// The Time Stamping oid (1.3.6.1.5.5.7.3.8).
    /// </summary>
    public const string TimeStamping = "1.3.6.1.5.5.7.3.8";

    /// <summary>
    /// The File Recovery oid (1.3.6.1.4.1.311.10.3.4.1).
    /// </summary>
    public const string FileRecovery = "1.3.6.1.4.1.311.10.3.4.1";

    /// <summary>
    /// The SPC Relaxed PE Marker Check oid (1.3.6.1.4.1.311.2.6.1).
    /// </summary>
    public const string SpcRelaxedPeMarkerCheck = "1.3.6.1.4.1.311.2.6.1";

    /// <summary>
    /// The Endorsement Key Certificate oid (2.23.133.8.1).
    /// </summary>
    public const string EndorsementKeyCertificate = "2.23.133.8.1";

    /// <summary>
    /// The Spc Encrypted Digest Retry Count oid (1.3.6.1.4.1.311.2.6.2).
    /// </summary>
    public const string SpcEncryptedDigestRetryCount = "1.3.6.1.4.1.311.2.6.2";

    /// <summary>
    /// The Encrypting File System oid (1.3.6.1.4.1.311.10.3.4).
    /// </summary>
    public const string EncryptingFileSystem = "1.3.6.1.4.1.311.10.3.4";

    /// <summary>
    /// The Server Authentication oid (1.3.6.1.5.5.7.3.1).
    /// </summary>
    public const string ServerAuthentication = "1.3.6.1.5.5.7.3.1";

    /// <summary>
    /// The HAL Extension oid (1.3.6.1.4.1.311.61.5.1).
    /// </summary>
    public const string HalExtension = "1.3.6.1.4.1.311.61.5.1";

    /// <summary>
    /// The Secure Email oid (1.3.6.1.5.5.7.3.4).
    /// </summary>
    public const string SecureEmail = "1.3.6.1.5.5.7.3.4";

    /// <summary>
    /// The IP Security End System oid (1.3.6.1.5.5.7.3.5).
    /// </summary>
    public const string IpSecurityEndSystem = "1.3.6.1.5.5.7.3.5";

    /// <summary>
    /// The Root List Signer oid (1.3.6.1.4.1.311.10.3.9).
    /// </summary>
    public const string RootListSigner = "1.3.6.1.4.1.311.10.3.9";

    /// <summary>
    /// The Disallowed List oid (1.3.6.1.4.1.311.10.3.30).
    /// </summary>
    public const string DisallowedList = "1.3.6.1.4.1.311.10.3.30";

    /// <summary>
    /// The Revoked List Signer oid (1.3.6.1.4.1.311.10.3.19).
    /// </summary>
    public const string RevokedListSigner = "1.3.6.1.4.1.311.10.3.19";

    /// <summary>
    /// The Qualified Subordination oid (1.3.6.1.4.1.311.10.3.10).
    /// </summary>
    public const string QualifiedSubordination = "1.3.6.1.4.1.311.10.3.10";

    /// <summary>
    /// The Document Signing oid (1.3.6.1.4.1.311.10.3.12).
    /// </summary>
    public const string DocumentSigning = "1.3.6.1.4.1.311.10.3.12";

    /// <summary>
    /// The Protected Process Verification oid (1.3.6.1.4.1.311.10.3.24).
    /// </summary>
    public const string ProtectedProcessVerification = "1.3.6.1.4.1.311.10.3.24";

    /// <summary>
    /// The Document Encryption oid (1.3.6.1.4.1.311.80.1).
    /// </summary>
    public const string DocumentEncryption = "1.3.6.1.4.1.311.80.1";

    /// <summary>
    /// The Protected Process Light Verification oid (1.3.6.1.4.1.311.10.3.22).
    /// </summary>
    public const string ProtectedProcessLightVerification = "1.3.6.1.4.1.311.10.3.22";

    /// <summary>
    /// The Directory Service Email Replication oid (1.3.6.1.4.1.311.21.19).
    /// </summary>
    public const string DirectoryServiceEmailReplication = "1.3.6.1.4.1.311.21.19";

    /// <summary>
    /// The Private Key Archival oid (1.3.6.1.4.1.311.21.5).
    /// </summary>
    public const string PrivateKeyArchival = "1.3.6.1.4.1.311.21.5";

    /// <summary>
    /// The Digital Rights oid (1.3.6.1.4.1.311.10.5.1).
    /// </summary>
    public const string DigitalRights = "1.3.6.1.4.1.311.10.5.1";

    /// <summary>
    /// The Preview Build Signing oid (1.3.6.1.4.1.311.10.3.27).
    /// </summary>
    public const string PreviewBuildSigning = "1.3.6.1.4.1.311.10.3.27";

    /// <summary>
    /// The Certificate Request Agent oid (1.3.6.1.4.1.311.20.2.1).
    /// </summary>
    public const string CertificateRequestAgent = "1.3.6.1.4.1.311.20.2.1";

    /// <summary>
    /// The Platform Certificate oid (2.23.133.8.2).
    /// </summary>
    public const string PlatformCertificate = "2.23.133.8.2";

    /// <summary>
    /// The CTL Usage oid (1.3.6.1.4.1.311.20.1).
    /// </summary>
    public const string CTLUsage = "1.3.6.1.4.1.311.20.1";

    /// <summary>
    /// The OSCSP Signing oid (1.3.6.1.5.5.7.3.9).
    /// </summary>
    public const string OcspSigning = "1.3.6.1.5.5.7.3.9";

    /// <summary>
    /// The Code signing oid (1.3.6.1.5.5.7.3.3).
    /// </summary>
    public const string CodeSigning = "1.3.6.1.5.5.7.3.3";

    /// <summary>
    /// The Microsoft Trust List Signing oid (1.3.6.1.4.1.311.10.3.1).
    /// </summary>
    public const string MicrosoftTrustListSigning = "1.3.6.1.4.1.311.10.3.1";

    /// <summary>
    /// The Microsoft Time Stamping oid (1.3.6.1.4.1.311.10.3.2).
    /// </summary>
    public const string MicrosoftTimeStamping = "1.3.6.1.4.1.311.10.3.2";

    /// <summary>
    /// The Microsoft Publisher oid (1.3.6.1.4.1.311.76.8.1).
    /// </summary>
    public const string MicrosoftPublisher = "1.3.6.1.4.1.311.76.8.1";

    /// <summary>
    /// The Client Authentication oid (1.3.6.1.5.5.7.3.2).
    /// </summary>
    public const string ClientAuthentication = "1.3.6.1.5.5.7.3.2";

    /// <summary>
    /// The Lifetime Signing oid (1.3.6.1.4.1.311.10.3.13).
    /// </summary>
    public const string LifetimeSigning = "1.3.6.1.4.1.311.10.3.13";

    /// <summary>
    /// The Any Purpose oid (2.5.29.37.0).
    /// </summary>
    public const string AnyPurpose = "2.5.29.37.0";

    /// <summary>
    /// The Domain Name System Server Trust oid (1.3.6.1.4.1.311.64.1.1).
    /// </summary>
    public const string DomainNameSystemServerTrust = "1.3.6.1.4.1.311.64.1.1";

    /// <summary>
    /// The OEM Windows System Component Verification oid (1.3.6.1.4.1.311.10.3.7).
    /// </summary>
    public const string OemWindowsSystemComponentVerification = "1.3.6.1.4.1.311.10.3.7";

    /// <summary>
    /// The Subject Key Identifier oid (2.5.29.14).
    /// </summary>
    public const string SubjectKeyIdentifier = "2.5.29.14";

    public const string MacAddress = "1.3.6.1.1.1.1.22";

    /// <summary>
    /// The Authority Key Identifier oid (2.5.29.35).
    /// </summary>
    public const string AuthorityKeyIdentifier = "2.5.29.35";

    /// <summary>
    /// A DSA certificate (1.2.840.10040.4.1).
    /// </summary>
    public const string Dsa = "1.2.840.10040.4.1";

    /// <summary>
    /// An RSA certificate (1.2.840.113549.1.1.1).
    /// </summary>
    public const string Rsa = "1.2.840.113549.1.1.1";

    /// <summary>
    /// An Elliptical Curve certificate (1.2.840.113549.1.1.1).
    /// </summary>
    public const string Ec = "1.2.840.10045.2.1";

    /// <summary>
    /// A user principal name. (1.3.6.1.4.1.311.20.2.3).
    /// </summary>
    public const string UserPrincipalName = "1.3.6.1.4.1.311.20.2.3";
}