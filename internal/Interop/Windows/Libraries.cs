using System.Diagnostics.CodeAnalysis;

#pragma warning disable SA1307, S101, SA1202, S3903
internal static partial class Interop
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore")]
    [SuppressMessage("Critical Code Smell", "S3218:Inner class members should not shadow outer class \"static\" or type members")]
    internal static partial class Libraries
    {
        internal const string Activeds = "activeds.dll";
        internal const string Advapi32 = "advapi32.dll";
        internal const string Authz = "authz.dll";
        internal const string BCrypt = "BCrypt.dll";
        internal const string Credui = "credui.dll";
        internal const string Crypt32 = "crypt32.dll";
        internal const string CryptUI = "cryptui.dll";
        internal const string Dnsapi = "dnsapi.dll";
        internal const string Dsrole = "dsrole.dll";
        internal const string Gdi32 = "gdi32.dll";
        internal const string HttpApi = "httpapi.dll";
        internal const string IpHlpApi = "iphlpapi.dll";
        internal const string Kernel32 = "kernel32.dll";
        internal const string Logoncli = "logoncli.dll";
        internal const string Mswsock = "mswsock.dll";
        internal const string NCrypt = "ncrypt.dll";
        internal const string Netapi32 = "netapi32.dll";
        internal const string Netutils = "netutils.dll";
        internal const string NtDll = "ntdll.dll";
        internal const string Odbc32 = "odbc32.dll";
        internal const string Ole32 = "ole32.dll";
        internal const string OleAut32 = "oleaut32.dll";
        internal const string Pdh = "pdh.dll";
        internal const string Secur32 = "secur32.dll";
        internal const string Shell32 = "shell32.dll";
        internal const string SspiCli = "sspicli.dll";
        internal const string User32 = "user32.dll";
        internal const string Version = "version.dll";
        internal const string WebSocket = "websocket.dll";
        internal const string Wevtapi = "wevtapi.dll";
        internal const string WinHttp = "winhttp.dll";
        internal const string WinMM = "winmm.dll";
        internal const string Wkscli = "wkscli.dll";
        internal const string Wldap32 = "wldap32.dll";
        internal const string Ws2_32 = "ws2_32.dll";
        internal const string Wtsapi32 = "wtsapi32.dll";
        internal const string MsQuic = "msquic.dll";
        internal const string HostPolicy = "hostpolicy.dll";
        internal const string Ucrtbase = "ucrtbase.dll";
        internal const string Xolehlp = "xolehlp.dll";
    }
}