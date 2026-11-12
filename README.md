# Linux Commands & Tools

A practical collection of command guides and small utilities for Linux and Windows. Everything in this project lives in this one repository; Windows-specific files are grouped in the `windows-edge-remover/` folder.

## Linux guides

- [Practical Linux Command Guide](PRACTICAL-LINUX-COMMAND-GUIDE.md) — navigation, files, search, processes, networking, packages, and archives.
- [Linux Troubleshooting Playbook](LINUX-TROUBLESHOOTING-PLAYBOOK.md) — steps for common disk, network, service, and performance problems.
- [Git and GitHub Workflow](GIT-AND-GITHUB-WORKFLOW.md) — save changes, use branches, push to GitHub, and recover safely.

## Linux tools

- [Linux Health Check](linux-health-check.sh) — read-only overview of disk space, memory, load, and failed systemd services. Run with `sh linux-health-check.sh`.
- [Linux Command Finder](linux-command-finder.sh) — interactive menu that prints common command examples for review. It does not execute them.

## Windows tool

- [Edge Remover for Windows](windows-edge-remover/) — Windows Forms app source and build script. It asks Windows Package Manager to uninstall Edge through its interactive interface. Build instructions are in the folder README.

Windows decides whether Edge can be removed on a device. This tool does not force-delete browser files or edit the registry. The app's `.exe` must be built on Windows with the .NET 8 SDK.

## Safety notes

- Read commands before running them, especially those using `sudo`, `rm`, `chmod`, or shell redirection.
- Review paths before moving or deleting files.
- Do not paste passwords, API keys, or private tokens into shell commands or public issue reports.
- Package management commands and Edge removal availability vary by operating system and region.

## Contributing

Corrections and clear examples are welcome. Explain what a command does, include a realistic example, and mention important risks or operating-system differences.
