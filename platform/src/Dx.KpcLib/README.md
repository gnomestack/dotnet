# GnomeStack.Dx.KpcLib

Provides a high level abstraction over the KpcLib which is a fork of KeePassLib
to make working with KeePass files easier.

## Usage

```csharp
var r = KpDatabase.Open(path, "weakest pw ever");
if (r.IsError)
{
    // handle error
    // var e = r.UnwrapError()
}

var db = r.Unwrap();
var entryOption = db.FindEntry("group1/group2/titleName");
if (entryOption.IsSome)
{
    Console.WriteLine(entryOption.Title);
    Console.WriteLine(entryOption.Notes);
}

```

### Using Deconstruction

```csharp
var (db, isError) = KpDatabase.Open(path, "weakest pw ever");
if (isError)
{
    // handle error
    // var e = r.UnwrapError()
}


var (entry, isSome)= db.FindEntry("group1/group2/titleName");
if (isSome)
{
    Console.WriteLine(entry.Title);
    Console.WriteLine(entry.Notes);
}

```

MIT License

Underlying KPCLib is licensed under LGPL v3
KPCLib source code is here: https://github.com/passxyz/KPCLib
