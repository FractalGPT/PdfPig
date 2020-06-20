﻿namespace UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter
{
    using Content;
    using Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Default Page Segmenter. All words are included in one block.
    /// </summary>
    public class DefaultPageSegmenter : IPageSegmenter
    {
        /// <summary>
        /// Create an instance of default page segmenter, <see cref="DefaultPageSegmenter"/>.
        /// </summary>
        public static DefaultPageSegmenter Instance { get; } = new DefaultPageSegmenter();

        /// <summary>
        /// Get the blocks using default options values.
        /// </summary>
        /// <param name="words">The page's words to generate text blocks for.</param>
        public IReadOnlyList<TextBlock> GetBlocks(IEnumerable<Word> words)
        {
            return GetBlocks(words, new DefaultPageSegmenterOptions());
        }

        /// <summary>
        /// Get the text blocks using options.
        /// </summary>
        /// <param name="words">The page's words to generate text blocks for.</param>
        /// <param name="options">The <see cref="DefaultPageSegmenterOptions"/> to use.</param>
        /// <returns>The <see cref="TextBlock"/>s generated by the default method.</returns>
        public IReadOnlyList<TextBlock> GetBlocks(IEnumerable<Word> words, DlaOptions options)
        {
            if (options is DefaultPageSegmenterOptions dOptions)
            {
                if (words?.Any() != true)
                {
                    return EmptyArray<TextBlock>.Instance;
                }

                return new List<TextBlock>() { new TextBlock(new XYLeaf(words).GetLines(dOptions.WordSeparator), dOptions.LineSeparator) };
            }
            else
            {
                throw new ArgumentException("Options provided must be of type " + nameof(DefaultPageSegmenterOptions) + ".", nameof(options));
            }
        }

        /// <summary>
        /// Default page segmenter options.
        /// </summary>
        public class DefaultPageSegmenterOptions : PageSegmenterOptions
        { }
    }
}