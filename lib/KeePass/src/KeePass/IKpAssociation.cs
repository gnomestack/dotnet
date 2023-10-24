using System;

namespace GnomeStack.KeePass;

public interface IKeePassAssociation
{
    string Window { get; set; }

    string KeystrokeSequence { get; set; }
}