using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libWyvernzora.Collections;

namespace libWyvernzora.Sandbox
{
    class GridNode : INode
    {
        public IGraph<INode> Graph
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IEdge<INode>> Edges
        {
            get { throw new NotImplementedException(); }
        }
    }
}
