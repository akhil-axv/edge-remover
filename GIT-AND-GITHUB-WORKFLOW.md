# Practical Git and GitHub Workflow

A compact guide to everyday Git work: start a project, save changes, work with branches, and recover from common mistakes.

## 1. Configure Git once

Set the name and email that should appear on your commits:

```bash
git config --global user.name "Your Name"
git config --global user.email "you@example.com"
```

Check your settings:

```bash
git config --global --list
```

## 2. Start or download a project

Start tracking the current folder:

```bash
git init
git status
```

Download an existing repository:

```bash
git clone https://github.com/OWNER/REPOSITORY.git
cd REPOSITORY
```

## 3. Save a change

A simple save cycle:

```bash
git status
git diff
git add path/to/file
git diff --staged
git commit -m "Describe the change"
```

Use `git add -A` to stage all changes in the current repository. Review `git diff --staged` before committing so you know exactly what will be saved.

Write commit messages that describe the change, such as `Add search to command list` or `Fix mobile navigation spacing`.

## 4. Work on a separate branch

Create and switch to a branch:

```bash
git switch -c add-command-examples
```

See the current branch and working tree:

```bash
git status --short --branch
```

Switch back to the main branch:

```bash
git switch main
```

Bring the latest main branch changes into your current branch:

```bash
git fetch origin
git merge origin/main
```

## 5. Connect to GitHub and share a branch

If you created a new GitHub repository, add it as the remote:

```bash
git remote add origin https://github.com/OWNER/REPOSITORY.git
git branch -M main
git push -u origin main
```

For a branch in an existing repository:

```bash
git push -u origin add-command-examples
```

Open a pull request on GitHub to propose merging your branch. Describe what changed and how you checked it.

After the pull request is merged:

```bash
git switch main
git pull
git branch -d add-command-examples
```

## 6. Inspect history

```bash
git log --oneline --decorate -10
git show COMMIT
git diff main..add-command-examples
```

Replace `COMMIT` with a commit hash or branch name to inspect a specific change.

## 7. Undo common mistakes safely

Unstage a file while keeping its edits:

```bash
git restore --staged path/to/file
```

Discard edits to a file that have not been committed:

```bash
git diff path/to/file
git restore path/to/file
```

The second command permanently discards the file's uncommitted edits. Review the diff first.

Make a new commit that reverses an earlier commit:

```bash
git revert COMMIT
```

Reverting creates a new commit and is usually the safer way to undo a commit that has already been shared.

## 8. Add files you do not want to track

Put patterns in a `.gitignore` file, for example:

```gitignore
.env
.env.*
!.env.example
node_modules/
.DS_Store
*.log
```

Never commit passwords, API keys, private tokens, or other secrets. If a secret was committed, removing the file in a later commit does not erase it from history; revoke or rotate the exposed secret.

## Quick checklist before pushing

- `git status` shows only the files you intend to share.
- `git diff --staged` contains no accidental edits or secrets.
- The commit message explains the change.
- You are pushing the intended branch: `git status --short --branch`.
- Your changes are committed before you run `git push`.

Commands can vary slightly with Git versions and repository policies. Run `git COMMAND --help` for detailed options.
