# Practical Linux Command Guide

A beginner-friendly reference for common Linux tasks. Commands are grouped by job, with examples you can adapt.

> Tip: start with commands that inspect or preview. Read a command carefully before running it, especially when it uses `sudo`, `rm`, `>`, or `find -delete`.

## Find your way around

| Task | Command | Example |
|---|---|---|
| Show current directory | `pwd` | `pwd` |
| List files, including hidden ones | `ls -la` | `ls -la ~/Downloads` |
| Change directory | `cd` | `cd ~/projects` |
| Go to the parent directory | `cd ..` | `cd ..` |
| Create a directory | `mkdir -p` | `mkdir -p projects/demo/src` |

## Read and search files

| Task | Command | Example |
|---|---|---|
| Print a small file | `cat` | `cat notes.txt` |
| Read a long file page by page | `less` | `less /var/log/syslog` |
| Show the first or last lines | `head` / `tail` | `tail -n 50 app.log` |
| Follow a log as it changes | `tail -f` | `tail -f app.log` |
| Search text in files | `grep -n` | `grep -n "ERROR" app.log` |
| Find files by name | `find` | `find . -type f -name "*.md"` |

Use `grep -R "TODO" src/` to search a directory tree. Add `-i` for case-insensitive matching.

## Manage files and directories

| Task | Command | Example |
|---|---|---|
| Copy a file | `cp` | `cp notes.txt notes.bak` |
| Copy a directory | `cp -r` | `cp -r site/ site-backup/` |
| Move or rename | `mv` | `mv draft.txt final.txt` |
| Remove a file | `rm` | `rm old.tmp` |
| Remove an empty directory | `rmdir` | `rmdir empty-folder` |

Check the source and destination before using `mv` or `rm`. Recursive deletion (`rm -r`) can remove many files at once; avoid running it on paths you have not checked.

## Understand files and disk use

| Task | Command | Example |
|---|---|---|
| Show file type | `file` | `file download.bin` |
| Show size in readable units | `du -sh` | `du -sh ~/Downloads` |
| Check free disk space | `df -h` | `df -h` |
| Show permissions and ownership | `ls -l` | `ls -l script.sh` |
| Change executable permission | `chmod +x` | `chmod +x script.sh` |

## Processes and system status

| Task | Command | Example |
|---|---|---|
| Show processes | `ps aux` | `ps aux` |
| Find a process by name | `pgrep` | `pgrep -a firefox` |
| Monitor processes interactively | `top` | `top` |
| Show memory use | `free -h` | `free -h` |
| Show system uptime | `uptime` | `uptime` |

Use `kill PID` only after identifying the process. Try a normal stop before using stronger signals.

## Networking

| Task | Command | Example |
|---|---|---|
| Show network interfaces and addresses | `ip addr` | `ip addr` |
| Test whether a host responds | `ping` | `ping -c 4 example.com` |
| Make an HTTP request | `curl` | `curl -I https://example.com` |
| Show listening ports | `ss -tulpn` | `ss -tulpn` |

## Packages (choose the section for your distribution)

### Debian and Ubuntu

```bash
sudo apt update
apt search PACKAGE
sudo apt install PACKAGE
sudo apt remove PACKAGE
```

### Fedora

```bash
sudo dnf check-update
dnf search PACKAGE
sudo dnf install PACKAGE
sudo dnf remove PACKAGE
```

### Arch Linux

```bash
sudo pacman -Syu
pacman -Ss PACKAGE
sudo pacman -S PACKAGE
sudo pacman -R PACKAGE
```

Package names and commands can vary by distribution. Check the package manager's prompt before confirming changes.

## Archives

```bash
tar -czf archive.tar.gz folder/   # create a gzip-compressed archive
tar -tzf archive.tar.gz           # list its contents without extracting
tar -xzf archive.tar.gz           # extract into the current directory
unzip archive.zip                 # extract a ZIP archive
```

## Shell shortcuts

- Press **Tab** to complete a path or command.
- Press **Ctrl+C** to stop most foreground commands.
- Use the **up arrow** to revisit a previous command.
- Run `command --help` or `man command` for built-in documentation.
- Use `history` to review recent commands. Avoid pasting secrets into shell commands, since they may be saved in shell history.

## A safe workflow

1. Confirm where you are with `pwd`.
2. Inspect the target with `ls -la PATH`.
3. Preview searches before changing files.
4. Make a backup before bulk edits.
5. Read prompts from `sudo` and package managers before confirming.

Contributions that add clear examples or cover another Linux distribution are welcome.
