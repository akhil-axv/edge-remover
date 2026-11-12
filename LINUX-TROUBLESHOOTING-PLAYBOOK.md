# Linux Troubleshooting Playbook

A first-response guide for common desktop and server problems. These steps gather information before making changes.

## Start with the symptom

### The system feels slow

Inspect load, memory, and processes:

```bash
uptime
free -h
top
```

In `top`, press `q` to quit. Sort and inspect before stopping anything. A high load average alone does not prove a fault; compare it with the number of CPU cores and check whether the load is rising.

### A disk is full

Check mounted filesystems and large directories:

```bash
df -h
du -h --max-depth=1 "$HOME" 2>/dev/null | sort -h
```

On systems where `du` does not support `--max-depth`, use `du -sh "$HOME"/* 2>/dev/null`. Inspect specific directories before deleting files. Avoid deleting system files or package-manager data as a first response.

### A command is missing

Find whether a command exists and which package provides it:

```bash
command -v COMMAND
```

On Debian or Ubuntu, search with `apt search PACKAGE`; on Fedora, use `dnf search PACKAGE`; on Arch, use `pacman -Ss PACKAGE`. Check the package name and source before installing.

### A service will not start (systemd)

Read its status and recent logs:

```bash
systemctl status SERVICE --no-pager
journalctl -u SERVICE -n 100 --no-pager
```

Replace `SERVICE` with the unit name. Logs can contain private paths, usernames, or tokens; review them before sharing. Do not paste secrets into a public issue.

### A network connection fails

Check local connectivity first, then name resolution and the remote service:

```bash
ip addr
ip route
ping -c 3 1.1.1.1
getent hosts example.com
curl -I --max-time 10 https://example.com
```

A host may block ping, so a failed ping by itself does not prove the network is down. Do not change firewall rules until you know which connection is affected.

### A process is using a port

List listening sockets:

```bash
ss -tulpn
```

Use `sudo` only if you need to see process details owned by another user. Identify the program before stopping it; terminating an unknown process may interrupt other work.

### A file has unexpected permissions

Inspect first:

```bash
ls -l PATH
stat PATH
```

Change only the specific permission you need. Avoid broad recursive changes such as `chmod -R 777`; they can expose or break many files.

## Safe information to include in a help request

Include the Linux distribution and version, the exact command you ran, the complete error message, and what you expected to happen. Remove passwords, access tokens, private hostnames, and personal data from logs and screenshots.

## Useful companion tools

- [Linux Health Check](linux-health-check.sh) gives a quick read-only overview.
- [Linux Command Finder](linux-command-finder.sh) prints common command examples for review; it does not execute them.
- [Practical Linux Command Guide](PRACTICAL-LINUX-COMMAND-GUIDE.md) is a general command reference.

Commands and options vary by distribution. Check the manual page (`man COMMAND`) before using an unfamiliar option.
