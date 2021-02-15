namespace WaifuDriver
{
    public interface INavigator<TNode>
    {
        float HeuristicDistance(TNode start, TNode end);
        float WeightFunction(TNode fromCoord, TNode toCoord, TNode cameFromCoord);
        void VisitNodeNeighbours(INodeVisitor<TNode> nodeVisitor, TNode node);
    }
}