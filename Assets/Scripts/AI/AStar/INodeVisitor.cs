namespace WaifuDriver
{
    public interface INodeVisitor<TNode>
    {
        void VisitNode(TNode node);
    }
}