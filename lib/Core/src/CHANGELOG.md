# CHANGE LOG

## 0.1.1 initial creation

- Add Symbol struct for js/ruby like symbols of interned strings.
- Add Fs.ChangeOwner function to (chown) change the owner of a file.
- Add Fs.ChangeMode function to (chmod) change the mode of a file.
- Add Diagnostics.PsPathRegistry to store executable paths and
  and fallback paths to search for executables for different platforms.
  - Useful for post installation of executables.
  - Useful for common install locations for user folders or system folders.
  - Useful for overriding the default location for an executable through
    environment variables or lookup paths.

## 0.1.0 initial creation
