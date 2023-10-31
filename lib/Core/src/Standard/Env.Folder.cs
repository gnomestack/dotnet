using System;

namespace GnomeStack.Standard;

public enum KnownDirectoryOption
{
    None = 0,
    Create = 1,
    DoNotVerify = 2,
    DoNotVerifyCreate = 3,
}

public static partial class Env
{
    public static string Directory(KnownDirectory directory)
    {
        var dir = (System.Environment.SpecialFolder)directory;
        return Environment.GetFolderPath(dir);
    }

    public static string Directory(KnownDirectory directory, KnownDirectoryOption option)
    {
        return Environment.GetFolderPath((Environment.SpecialFolder)directory, (Environment.SpecialFolderOption)option);
    }
}

// ReSharper disable InconsistentNaming

/// <summary>Specifies enumerated constants used to retrieve directory paths to system special folders.
/// <see cref="KnownDirectory"/> is an extended version of <see cref="Environment.SpecialFolder"/>
/// to enable additional friendly folder mappings and to support additional unix folders.
/// </summary>
#pragma warning disable SA1201 // in this case we want to keep the enum in the same file
public enum KnownDirectory
{
    /// <summary>The logical Desktop rather than the physical file system location.</summary>
    Desktop = 0,

    /// <summary>The directory that contains the user's program groups.</summary>
    Programs = 2,

    /// <summary>The My Documents folder. This member is equivalent to  <see cref="F:System.Environment.SpecialFolder.Personal" />.</summary>
    MyDocuments = 5,

    /// <summary>The My Documents folder. This member is equivalent to  <see cref="F:System.Environment.SpecialFolder.Personal" />.</summary>
    Documents = 5,

    /// <summary>The directory that serves as a common repository for documents.  This member is equivalent to  <see cref="F:System.Environment.SpecialFolder.MyDocuments" />.</summary>
    Personal = 5,

    /// <summary>The directory that serves as a common repository for the user's favorite items.</summary>
    Favorites = 6,

    /// <summary>The directory that corresponds to the user's Startup program group. The system starts these programs whenever a user logs on or starts Windows.</summary>
    Startup = 7,

    /// <summary>The directory that contains the user's most recently used documents.</summary>
    Recent = 8,

    /// <summary>The directory that contains the Send To menu items.</summary>
    SendTo = 9,

    /// <summary>The directory that contains the Start menu items.</summary>
    StartMenu = 11, // 0x0000000B

    /// <summary>The user's Music folder. This member is equivalent to <see cref="F:Std.OS.SpecialDirectory.MyMusic" />.</summary>
    Music = 13,

    /// <summary>The My Music folder.</summary>
    MyMusic = 13, // 0x0000000D

    /// <summary>The file system directory that serves as a repository for videos that belong to a user.</summary>
    MyVideos = 14, // 0x0000000E

    /// <summary>The file system directory that serves as a repository for videos that belong to a user.</summary>
    Videos = 14, // 0x0000000E

    /// <summary>The directory used to physically store file objects on the desktop. Do not confuse this directory with the desktop folder itself, which is a virtual folder.</summary>
    DesktopDirectory = 16, // 0x00000010

    /// <summary>The My Computer folder. When passed to the <see cref="F:Std.Os.Env.Directory()" /> method, the <see cref="MyComputer" /> enumeration member always yields the empty string ("") because no path is defined for the My Computer folder.</summary>
    MyComputer = 17, // 0x00000011

    /// <summary>A file system directory that contains the link objects that may exist in the My Network Places virtual folder.</summary>
    NetworkShortcuts = 19, // 0x00000013

    /// <summary>A virtual folder that contains fonts.</summary>
    Fonts = 20, // 0x00000014

    /// <summary>The directory that serves as a common repository for document templates.</summary>
    Templates = 21, // 0x00000015

    /// <summary>The file system directory that contains the programs and folders that appear on the Start menu for all users.</summary>
    CommonStartMenu = 22, // 0x00000016

    /// <summary>A folder for components that are shared across applications.</summary>
    CommonPrograms = 23, // 0x00000017

    /// <summary>The file system directory that contains the programs that appear in the Startup folder for all users.</summary>
    CommonStartup = 24, // 0x00000018

    /// <summary>The file system directory that contains files and folders that appear on the desktop for all users.</summary>
    CommonDesktopDirectory = 25, // 0x00000019

    /// <summary>
    /// The directory that serves as a common repository for application-specific data for
    /// the current roaming user. A roaming user works on more than one computer on a network. A roaming user's profile is
    /// kept on a server on the network and is loaded onto a system when the user logs on.
    /// </summary>
    ApplicationData = 26, // 0x0000001A

    /// <summary>
    /// The directory that serves as a common repository for application-specific data for
    /// the current roaming user. A roaming user works on more than one computer on a network. A roaming user's profile is
    /// kept on a server on the network and is loaded onto a system when the user logs on.
    /// </summary>
    HomeConfig = 26, // 0x0000001A

    /// <summary>The file system directory that contains the link objects that can exist in the Printers virtual folder.</summary>
    PrinterShortcuts = 27, // 0x0000001B

    /// <summary>The directory that serves as a common repository for application-specific data that is used by the current, non-roaming user.</summary>
    LocalApplicationData = 28, // 0x0000001C

    /// <summary>The directory that serves as a common repository for application-specific data that is used by the current, non-roaming user.</summary>
    HomeData = 28, // 0x0000001C

    /// <summary>The directory that serves as a common repository for temporary Internet files.</summary>
    InternetCache = 32, // 0x00000020

    /// <summary>The directory that serves as a common repository for Internet cookies.</summary>
    Cookies = 33, // 0x00000021

    /// <summary>The directory that serves as a common repository for Internet history items.</summary>
    History = 34, // 0x00000022

    /// <summary>The directory that serves as a common repository for application-specific data that is used by all users.</summary>
    CommonApplicationData = 35, // 0x00000023

    /// <summary>The Windows directory or SYSROOT. This corresponds to the <c>%windir%</c> or <c>%SYSTEMROOT%</c> environment variables.</summary>
    Windows = 36, // 0x00000024

    /// <summary>The System directory.</summary>
    System = 37, // 0x00000025

    /// <summary>The program files directory.
    /// In a non-x86 process, passing <see cref="F:System.Environment.SpecialFolder.ProgramFiles" /> to
    /// the <see cref="M:System.Environment.GetFolderPath(System.Environment.SpecialFolder)" /> method returns
    /// the path for non-x86 programs. To get the x86 program files directory in a non-x86 process,
    /// use the <see cref="F:System.Environment.SpecialFolder.ProgramFilesX86" /> member.
    /// </summary>
    ProgramFiles = 38, // 0x00000026

    /// <summary>The My Pictures folder.</summary>
    MyPictures = 39, // 0x00000027

    /// <summary>The user's profile folder. Applications should not create files or folders at this level;
    /// they should put their data under the locations referred to by <see cref="F:System.Environment.SpecialFolder.ApplicationData" />.
    /// </summary>
    UserProfile = 40, // 0x00000028

    /// <summary>
    /// The user's home folder. Applications should not create files or folders at this level; they should put their data under
    /// the locations referred to by <see cref="F:System.Environment.SpecialFolder.ApplicationData" />.
    /// </summary>
    Home = 40, // 0x00000028

    /// <summary>The Windows System folder.</summary>
    SystemX86 = 41, // 0x00000029

    /// <summary>The x86 Program Files folder.</summary>
    ProgramFilesX86 = 42, // 0x0000002A

    /// <summary>The directory for components that are shared across applications.
    /// To get the x86 common program files directory in a non-x86 process, use the <see cref="F:System.Environment.SpecialFolder.ProgramFilesX86" /> member.</summary>
    CommonProgramFiles = 43, // 0x0000002B

    /// <summary>The Program Files folder.</summary>
    CommonProgramFilesX86 = 44, // 0x0000002C

    /// <summary>The file system directory that contains the templates that are available to all users.</summary>
    CommonTemplates = 45, // 0x0000002D

    /// <summary>The file system directory that contains documents that are common to all users.</summary>
    CommonDocuments = 46, // 0x0000002E

    /// <summary>The file system directory that contains administrative tools for all users of the computer.</summary>
    CommonAdminTools = 47, // 0x0000002F

    /// <summary>The file system directory that is used to store administrative tools for an individual user. The Microsoft Management Console (MMC) will save customized consoles to this directory, and it will roam with the user.</summary>
    AdminTools = 48, // 0x00000030

    /// <summary>The file system directory that serves as a repository for music files common to all users.</summary>
    CommonMusic = 53, // 0x00000035

    /// <summary>The file system directory that serves as a repository for image files common to all users.</summary>
    CommonPictures = 54, // 0x00000036

    /// <summary>The file system directory that serves as a repository for video files common to all users.</summary>
    CommonVideos = 55, // 0x00000037

    /// <summary>The file system directory that contains resource data.</summary>
    Resources = 56, // 0x00000038

    /// <summary>The file system directory that contains localized resource data.</summary>
    LocalizedResources = 57, // 0x00000039

    /// <summary>This value is recognized in Windows Vista for backward compatibility, but the special folder itself is no longer used.</summary>
    CommonOemLinks = 58, // 0x0000003A

    /// <summary>The file system directory that acts as a staging area for files waiting to be written to a CD.</summary>
    CDBurning = 59, // 0x0000003B

    Downloads = 1000, // 0x0000003C
}