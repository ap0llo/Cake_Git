﻿using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Git.Extensions;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable UnusedMember.Global

namespace Cake.Git
{
    public static partial class GitAliases
    {
        /// <summary>
        /// Gets a git repository's remotes.
        /// </summary>
        /// <example>
        /// <code>
        ///     var remotes = GitRemotes("c:/temp/cake");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="repositoryDirectoryPath">Path to repository.</param>
        /// <returns>A list of <see cref="GitRemote"/> objects for the specified repository.</returns>
        /// <exception cref="ArgumentNullException">If any of the parameters are null.</exception>
        /// <exception cref="RepositoryNotFoundException">If path doesn't exist.</exception>
        [CakeMethodAlias]
        [CakeAliasCategory("Remotes")]
        public static IReadOnlyList<GitRemote> GitRemotes(this ICakeContext context, DirectoryPath repositoryDirectoryPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (repositoryDirectoryPath == null)
            {
                throw new ArgumentNullException(nameof(repositoryDirectoryPath));
            }

            if (!context.FileSystem.Exist(repositoryDirectoryPath))
            {
                throw new RepositoryNotFoundException($"Path '{repositoryDirectoryPath}' doesn't exists.");
            }

            return context.UseRepository(
              repositoryDirectoryPath,
              repository => repository.Network.Remotes.Select(remote => new GitRemote(remote.Name, remote.PushUrl, remote.Url)).ToList()
              );
        }

        /// <summary>
        /// Gets the specified remote from a git repository.
        /// </summary>
        /// <example>
        /// <code>
        ///     var remotes = GitRemote("c:/temp/cake", "origin");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="repositoryDirectoryPath">Path to repository.</param>
        /// <param name="remoteName">The name of the remote to get.</param>
        /// <returns>Information about the requested remote or null if no matching remote was found.</returns>
        /// <exception cref="ArgumentNullException">If any of the parameters are null.</exception>
        /// <exception cref="RepositoryNotFoundException">If path doesn't exist.</exception>
        [CakeMethodAlias]
        [CakeAliasCategory("Remotes")]
        public static GitRemote GitRemote(this ICakeContext context, DirectoryPath repositoryDirectoryPath, string remoteName)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (repositoryDirectoryPath == null)
            {
                throw new ArgumentNullException(nameof(repositoryDirectoryPath));
            }

            if (!context.FileSystem.Exist(repositoryDirectoryPath))
            {
                throw new RepositoryNotFoundException($"Path '{repositoryDirectoryPath}' doesn't exists.");
            }

            return context.UseRepository(
              repositoryDirectoryPath,
              repository => repository.Network.Remotes[remoteName] is { } remote ? new GitRemote(remote.Name, remote.PushUrl, remote.Url) : null
              );
        }
    }
}