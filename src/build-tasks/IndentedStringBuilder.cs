// Copyright (C) 2015-2024 The EpicChain Project.
//
// IndentedStringBuilder.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System;
using System.IO;
using System.Text;

// This code was originally published as part of EFCore (https://github.com/dotnet/efcore/blob/main/src/EFCore/Infrastructure/IndentedStringBuilder.cs)

namespace EpicChain.BuildTasks
{
    /// <summary>
    ///     <para>
    ///         A thin wrapper over <see cref="StringBuilder" /> that adds indentation to each line built.
    ///     </para>
    ///     <para>
    ///         This type is typically used by database providers (and other extensions). It is generally
    ///         not used in application code.
    ///     </para>
    /// </summary>
    class IndentedStringBuilder
    {
        private const byte IndentSize = 4;
        private byte _indent;
        private bool _indentPending = true;

        private readonly StringBuilder _stringBuilder = new StringBuilder();

        /// <summary>
        ///     The current length of the built string.
        /// </summary>
        public virtual int Length
            => _stringBuilder.Length;

        /// <summary>
        ///     Appends the current indent and then the given string to the string being built.
        /// </summary>
        /// <param name="value"> The string to append. </param>
        /// <returns> This builder so that additional calls can be chained. </returns>
        public virtual IndentedStringBuilder Append(string value)
        {
            DoIndent();

            _stringBuilder.Append(value);

            return this;
        }

        /// <summary>
        ///     Appends a new line to the string being built.
        /// </summary>
        /// <returns> This builder so that additional calls can be chained. </returns>
        public virtual IndentedStringBuilder AppendLine()
        {
            AppendLine(string.Empty);

            return this;
        }

        /// <summary>
        ///     <para>
        ///         Appends the current indent, the given string, and a new line to the string being built.
        ///     </para>
        ///     <para>
        ///         If the given string itself contains a new line, the part of the string after that new line will not be indented.
        ///     </para>
        /// </summary>
        /// <param name="value"> The string to append. </param>
        /// <returns> This builder so that additional calls can be chained. </returns>
        public virtual IndentedStringBuilder AppendLine(string value)
        {
            if (value.Length != 0)
            {
                DoIndent();
            }

            _stringBuilder.AppendLine(value);

            _indentPending = true;

            return this;
        }

        /// <summary>
        ///     Separates the given string into lines, and then appends each line, prefixed
        ///     by the current indent and followed by a new line, to the string being built.
        /// </summary>
        /// <param name="value"> The string to append. </param>
        /// <param name="skipFinalNewline"> If true, then the terminating new line is not added after the last line. </param>
        /// <returns> This builder so that additional calls can be chained. </returns>
        public virtual IndentedStringBuilder AppendLines(string value, bool skipFinalNewline = false)
        {
            using (var reader = new StringReader(value))
            {
                var first = true;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        AppendLine();
                    }

                    if (line.Length != 0)
                    {
                        Append(line);
                    }
                }
            }

            if (!skipFinalNewline)
            {
                AppendLine();
            }

            return this;
        }

        /// <summary>
        ///     Resets this builder ready to build a new string.
        /// </summary>
        /// <returns> This builder so that additional calls can be chained. </returns>
        public virtual IndentedStringBuilder Clear()
        {
            _stringBuilder.Clear();
            _indent = 0;

            return this;
        }

        /// <summary>
        ///     Increments the indent.
        /// </summary>
        /// <returns> This builder so that additional calls can be chained. </returns>
        public virtual IndentedStringBuilder IncrementIndent()
        {
            _indent++;

            return this;
        }

        /// <summary>
        ///     Decrements the indent.
        /// </summary>
        /// <returns> This builder so that additional calls can be chained. </returns>
        public virtual IndentedStringBuilder DecrementIndent()
        {
            if (_indent > 0)
            {
                _indent--;
            }

            return this;
        }

        /// <summary>
        ///     Creates a scoped indenter that will increment the indent, then decrement it when disposed.
        /// </summary>
        /// <returns> An indenter. </returns>
        public virtual IDisposable Indent()
            => new Indenter(this);

        /// <summary>
        ///     Returns the built string.
        /// </summary>
        /// <returns> The built string. </returns>
        public override string ToString()
            => _stringBuilder.ToString();

        private void DoIndent()
        {
            if (_indentPending && _indent > 0)
            {
                _stringBuilder.Append(' ', _indent * IndentSize);
            }

            _indentPending = false;
        }

        private sealed class Indenter : IDisposable
        {
            private readonly IndentedStringBuilder _stringBuilder;

            public Indenter(IndentedStringBuilder stringBuilder)
            {
                _stringBuilder = stringBuilder;

                _stringBuilder.IncrementIndent();
            }

            public void Dispose()
                => _stringBuilder.DecrementIndent();
        }
    }
}
