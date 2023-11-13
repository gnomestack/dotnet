using System.Diagnostics.CodeAnalysis;

namespace GnomeStack.Data.Mssql.Management;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum AzDbServiceObjective
{
    Basic,
    S0,
    S1,
    S2,
    S3,
    S4,
    S6,
    S7,
    S9,
    S12,
    P1,
    P2,
    P4,
    P6,
    P11,
    P15,
    GP_Gen5_n,
    GP_Fsv2_n,
    GP_S_Gen5_n,
    BC_Gen5_n,
    BC_M_n,
    HS_Gen5_n,
    HS_PRMS_n,
    HS_MOPRMS_n,
}