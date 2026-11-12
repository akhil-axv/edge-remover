#!/bin/sh
# Read-only Linux health overview. No root access, installs, or network calls.
# Usage: sh linux-health-check.sh

set -u

checks_ok=0
checks_warn=0
checks_info=0

if [ -t 1 ] && [ -z "${NO_COLOR:-}" ]; then
  green=$(printf '\033[32m')
  yellow=$(printf '\033[33m')
  blue=$(printf '\033[34m')
  reset=$(printf '\033[0m')
else
  green=''
  yellow=''
  blue=''
  reset=''
fi

ok() {
  printf '%s[OK]%s %s\n' "$green" "$reset" "$1"
  checks_ok=$((checks_ok + 1))
}

warn() {
  printf '%s[CHECK]%s %s\n' "$yellow" "$reset" "$1"
  checks_warn=$((checks_warn + 1))
}

info() {
  printf '%s[INFO]%s %s\n' "$blue" "$reset" "$1"
  checks_info=$((checks_info + 1))
}

section() {
  printf '\n%s\n' "$1"
}

printf 'Linux Health Check\n'
printf 'Read-only overview; thresholds are simple prompts to investigate, not diagnoses.\n'

section "System"
if [ -r /etc/os-release ]; then
  os_name=$(awk -F= '$1 == "PRETTY_NAME" {gsub(/^"|"$/, "", $2); print $2; exit}' /etc/os-release)
else
  os_name=''
fi
[ -n "$os_name" ] && info "OS: $os_name" || info "OS: $(uname -s)"
info "Kernel: $(uname -r)"
if command -v uptime >/dev/null 2>&1; then
  info "Uptime: $(uptime | sed 's/^[[:space:]]*//')"
fi

section "Root filesystem"
if command -v df >/dev/null 2>&1; then
  disk_pct=$(df -P / 2>/dev/null | awk 'NR == 2 {gsub(/%/, "", $5); print $5}')
  case "$disk_pct" in
    ''|*[!0-9]*) info "Could not read root filesystem usage." ;;
    *)
      if [ "$disk_pct" -ge 90 ]; then
        warn "Root filesystem is ${disk_pct}% full. Check large files before deleting anything."
      elif [ "$disk_pct" -ge 80 ]; then
        warn "Root filesystem is ${disk_pct}% full; consider reviewing disk usage."
      else
        ok "Root filesystem usage is ${disk_pct}%."
      fi
      ;;
  esac
else
  info "df is unavailable; skipped disk check."
fi

section "Memory"
if command -v free >/dev/null 2>&1; then
  memory_values=$(free -m 2>/dev/null | awk '/^Mem:/ { if (NF >= 7) print $7, $2; else print $4, $2; exit }')
  set -- $memory_values
  available_mb=${1:-}
  total_mb=${2:-}
  case "$available_mb:$total_mb" in
    *[!0-9:]*|:*) info "Could not read memory figures." ;;
    *)
      if [ "$total_mb" -gt 0 ]; then
        available_pct=$((available_mb * 100 / total_mb))
        if [ "$available_pct" -lt 10 ]; then
          warn "Available memory is about ${available_pct}% (${available_mb} MiB of ${total_mb} MiB)."
        else
          ok "Available memory is about ${available_pct}% (${available_mb} MiB of ${total_mb} MiB)."
        fi
      else
        info "Could not read total memory."
      fi
      ;;
  esac
else
  info "free is unavailable; skipped memory check."
fi

section "System load"
if [ -r /proc/loadavg ]; then
  load_1m=$(awk '{print $1}' /proc/loadavg)
  cpu_count=$(getconf _NPROCESSORS_ONLN 2>/dev/null || printf '1')
  case "$load_1m:$cpu_count" in
    *[!0-9.]*:*|:*|*:0) info "Could not read load average or CPU count." ;;
    *)
      overloaded=$(awk -v load="$load_1m" -v cpus="$cpu_count" 'BEGIN { print (load > cpus * 1.5) ? 1 : 0 }')
      if [ "$overloaded" -eq 1 ]; then
        warn "1-minute load is $load_1m across $cpu_count CPU(s); check top or uptime for busy processes."
      else
        ok "1-minute load is $load_1m across $cpu_count CPU(s)."
      fi
      ;;
  esac
else
  info "Load average is unavailable on this system."
fi

section "Failed systemd services"
if command -v systemctl >/dev/null 2>&1; then
  failed_units=$(systemctl --failed --no-legend --plain --no-pager 2>/dev/null || true)
  if [ -z "$failed_units" ]; then
    ok "No failed systemd services reported (or systemd is not managing this session)."
  else
    failed_count=$(printf '%s\n' "$failed_units" | awk 'NF {n++} END {print n+0}')
    warn "$failed_count failed systemd unit(s) reported. Inspect with: systemctl --failed"
  fi
else
  info "systemctl is unavailable; skipped service check."
fi

section "Summary"
printf 'OK: %s  Check: %s  Info: %s\n' "$checks_ok" "$checks_warn" "$checks_info"
if [ "$checks_warn" -gt 0 ]; then
  printf 'Review the flagged items and consult your distribution documentation before changing system settings.\n'
  exit 1
fi
printf 'No threshold warnings were found in the checks available on this system.\n'
exit 0
