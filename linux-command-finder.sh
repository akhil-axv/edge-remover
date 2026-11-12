#!/bin/sh
# Interactive, read-only command finder. Prints commands for you to review; never runs them.
set -u

if [ ! -t 0 ]; then
  printf 'Run this tool in an interactive terminal.\n' >&2
  exit 2
fi

while :; do
  printf '\nLinux Command Finder\n'
  printf 'Choose a task (q to quit):\n'
  printf '  1) Find files by name\n'
  printf '  2) Find text in files\n'
  printf '  3) Check disk space\n'
  printf '  4) Find large directories\n'
  printf '  5) Check listening ports\n'
  printf '  6) Follow a log file\n'
  printf '  7) Inspect a process\n'
  printf '  8) Archive a folder\n'
  printf '> '
  IFS= read -r choice || exit 0

  case "$choice" in
    q|Q) exit 0 ;;
    1) printf 'find . -type f -iname "*.txt"\n' ;;
    2) printf 'grep -Rni -- "search text" .\n' ;;
    3) printf 'df -h\n' ;;
    4) printf 'du -h --max-depth=1 . 2>/dev/null | sort -h\n' ;;
    5) printf 'ss -tulpn\n' ;;
    6) printf 'tail -f /path/to/file.log\n' ;;
    7) printf 'ps aux | less\n' ;;
    8) printf 'tar -czf archive.tar.gz folder/\n' ;;
    *) printf 'Choose a number from 1 to 8, or q to quit.\n' ;;
  esac
  printf '\nReview and edit the example before running it.\n'
done
