// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// libWyvernzora/EdgeBase.cs
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

using System;

// ReSharper disable CheckNamespace

namespace libWyvernzora.Collections
{
    /// <summary>
    ///     Represents an edge connecting two nodes in a GraphBase.
    /// </summary>
    /// <typeparam name="TNode">Type of nodes in the graph base.</typeparam>
    public interface IEdge<out TNode> where TNode : INode
    {
        /// <summary>
        ///     When implemented, gets the graph this edge belongs to.
        /// </summary>
        IGraph<TNode> Graph { get; }

        /// <summary>
        ///     When implemented, gets the origin node of the edge.
        /// </summary>
        TNode Origin { get; }

        /// <summary>
        ///     When implemented, gets the destination node of the edge.
        /// </summary>
        TNode Destination { get; }

        /// <summary>
        ///     When implemented, gets the weight of the edge.
        ///     If the IsWeighted property is false, returns 1.0.
        /// </summary>
        Double Weight { get; set; }

        /// <summary>
        ///     When implemented, gets a value indicating whether the edge is weighted.
        /// </summary>
        Boolean IsWeighted { get; }

        /// <summary>
        ///     When implemented, gets a value indicating whether the edge is directed.
        /// </summary>
        Boolean IsDirected { get; }
    }
}