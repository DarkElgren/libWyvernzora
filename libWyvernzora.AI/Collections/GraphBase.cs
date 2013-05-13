// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/GraphBase.cs
// --------------------------------------------------------------------------------
// Copyright (c) 2013, Jieni Luchijinzhou a.k.a Aragorn Wyvernzora
// 
// This file is a part of libWyvernzora.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to do 
// so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace libWyvernzora.Collections
{
    /// <summary>
    ///     Represents a graph structure.
    /// </summary>
    public interface IGraph<out TNode> where TNode : INode
    {
        /// <summary>
        ///     When implemented, gets the collection of graph nodes.
        /// </summary>
        IEnumerable<TNode> Nodes { get; }

        /// <summary>
        ///     When implemented, gets the collection of graph edges.
        /// </summary>
        IEnumerable<IEdge<INode>> Edges { get; }

        /// <summary>
        ///     When implemented, gets collection of outgoing edges for a node.
        /// </summary>
        /// <param name="node">Graph node whose outgoing edges to get.</param>
        /// <returns>IEnumerable of graph edges.</returns>
        IEnumerable<IEdge<INode>> GetEdges(INode node);

        /// <summary>
        ///     When implemented, gets collection of adjacent nodes for a node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        IEnumerable<TNode> GetAdjacentNodes(INode node);

        /// <summary>
        ///     When implemented, gets a specific edge from the graph.
        ///     If the edge does not exist, returns null.
        /// </summary>
        /// <param name="from">Starting node of the edge.</param>
        /// <param name="to">Ending node of the edge.</param>
        /// <returns>IEdge if the edge is found; null otherwise.</returns>
        IEdge<TNode> GetEdge(INode from, INode to);
    }
}