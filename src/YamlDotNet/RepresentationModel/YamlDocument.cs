//  This file is part of YamlDotNet - A .NET library for YAML.
//  Copyright (c) Antoine Aubry and contributors

//  Permission is hereby granted, free of charge, to any person obtaining a copy of
//  this software and associated documentation files (the "Software"), to deal in
//  the Software without restriction, including without limitation the rights to
//  use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//  of the Software, and to permit persons to whom the Software is furnished to do
//  so, subject to the following conditions:

//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.

//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel
{
    /// <summary>
    /// Represents an YAML document.
    /// </summary>
    [Serializable]
    public class YamlDocument
    {
        /// <summary>
        /// Gets or sets the root node.
        /// </summary>
        /// <value>The root node.</value>
        public YamlNode RootNode { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlDocument"/> class.
        /// </summary>
        public YamlDocument(YamlNode rootNode)
        {
            RootNode = rootNode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlDocument"/> class with a single scalar node.
        /// </summary>
        public YamlDocument(string rootNode)
        {
            RootNode = new YamlScalarNode(rootNode);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlDocument"/> class.
        /// </summary>
        internal YamlDocument(IParser parser)
        {
            var state = new DocumentLoadingState();

            parser.Expect<DocumentStart>();

            while (!parser.Accept<DocumentEnd>())
            {
                Debug.Assert(RootNode == null);
                RootNode = YamlNode.ParseNode(parser, state);

                if (RootNode is YamlAliasNode)
                {
                    throw new YamlException();
                }
            }

            state.ResolveAliases();

#if DEBUG
            foreach (var node in AllNodes)
            {
                if (node is YamlAliasNode)
                {
                    throw new InvalidOperationException("Error in alias resolution.");
                }
            }
#endif

            parser.Expect<DocumentEnd>();
        }

        /// <summary>
        /// Visitor that assigns anchors to nodes that are referenced more than once but have no anchor.
        /// </summary>
        private class AnchorAssigningVisitor : YamlVisitorBase
        {
            private readonly HashSet<string> existingAnchors = new HashSet<string>();
            private readonly Dictionary<YamlNode, bool> visitedNodes = new Dictionary<YamlNode, bool>();

            public void AssignAnchors(YamlDocument document)
            {
                existingAnchors.Clear();
                visitedNodes.Clear();

                document.Accept(this);

                var random = new Random();
                foreach (var visitedNode in visitedNodes)
                {
                    if (visitedNode.Value)
                    {
                        string anchor;
                        do
                        {
                            anchor = random.Next().ToString(CultureInfo.InvariantCulture);
                        } while (existingAnchors.Contains(anchor));
                        existingAnchors.Add(anchor);

                        visitedNode.Key.Anchor = anchor;
                    }
                }
            }

            private void VisitNode(YamlNode node)
            {
                if (string.IsNullOrEmpty(node.Anchor))
                {
                    bool isDuplicate;
                    if (visitedNodes.TryGetValue(node, out isDuplicate))
                    {
                        if (!isDuplicate)
                        {
                            visitedNodes[node] = true;
                        }
                    }
                    else
                    {
                        visitedNodes.Add(node, false);
                    }
                }
                else
                {
                    existingAnchors.Add(node.Anchor);
                }
            }

            public override void Visit(YamlScalarNode scalar)
            {
                // Do not assign anchors to scalars
            }

            public override void Visit(YamlMappingNode mapping)
            {
                VisitNode(mapping);
                base.Visit(mapping);
            }

            public override void Visit(YamlSequenceNode sequence)
            {
                VisitNode(sequence);
                base.Visit(sequence);
            }
        }

        private void AssignAnchors()
        {
            var visitor = new AnchorAssigningVisitor();
            visitor.AssignAnchors(this);
        }

        internal void Save(IEmitter emitter, bool assignAnchors = true)
        {
            if (assignAnchors)
            {
                AssignAnchors();
            }

            emitter.Emit(new DocumentStart());
            RootNode.Save(emitter, new EmitterState());
            emitter.Emit(new DocumentEnd(false));
        }

        /// <summary>
        /// Accepts the specified visitor by calling the appropriate Visit method on it.
        /// </summary>
        /// <param name="visitor">
        /// A <see cref="IYamlVisitor"/>.
        /// </param>
        public void Accept(IYamlVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Gets all nodes from the document.
        /// </summary>
        public IEnumerable<YamlNode> AllNodes
        {
            get
            {
                return RootNode.AllNodes;
            }
        }
    }
}