# Linux Commands & Tools

A practical, beginner-friendly collection of Linux command guides and small utilities. The goal is to help you solve common tasks quickly, understand what a command does, and avoid risky copy-paste.

## Guides

- [Practical Linux Command Guide](PRACTICAL-LINUX-COMMAND-GUIDE.md) — navigation, files, search, processes, networking, packages, and archives.
- [Git and GitHub Workflow](GIT-AND-GITHUB-WORKFLOW.md) — save changes, use branches, push to GitHub, and recover safely.

## Tools

### Linux Health Check

`linux-health-check.sh` is a small read-only shell script that gives a quick overview of disk space, memory, load, and failed systemd services.

Run it from this repository:

```bash
sh linux-health-check.sh
```

Or make it executable and run it directly:

```bash
chmod +x linux-health-check.sh
./linux-health-check.sh
```

It does not install packages, change settings, or require root access. It only reports checks supported by the current system. The output stays in your terminal; it does not upload or save system details.

## Safety notes

- Read a command before running it, especially commands using `sudo`, `rm`, `chmod`, or shell redirection.
- Review paths before moving or deleting files.
- Do not paste passwords, API keys, or private tokens into shell commands.
- Package management commands depend on your Linux distribution.

## Contributing

Corrections and clear examples are welcome. If you add a command, explain what it does, include a realistic example, and mention any important risks or distribution differences.
