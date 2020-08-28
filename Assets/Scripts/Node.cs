namespace Pathfinding_Astar
{
    public class Node
    {
        public enum NodeStates
        {
            WALKABLE,
            NOTWALKABLE,
            START,
            END,
            PATH,
            OPEN,
            CLOSED
        }

        public NodeStates _state;
        public bool _walkable
        {
            get
            {
                return _state == NodeStates.WALKABLE || _state == NodeStates.START || _state == NodeStates.END;
            }
        }
        public int _x;
        public int _y;

        public int _gCost;
        public int _hCost;
        public int _fCost
        {
            get
            {
                return _gCost + _hCost;
            }
        }

        public Node parent = null;

        public Node(NodeStates state, int x, int y)
        {
            _state = state;
            _x = x;
            _y = y;
        }
    }
}
