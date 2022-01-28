using System;
namespace vehiclepositions.Utilties
{
    class Node
    {
        internal int axis;
        internal double[] x;
        internal int id;
        internal bool visited;
        internal bool orientation;

        internal Node Parent, Left, Right;

        public Node(double[] x0, int axis0)
        {
            x = new double[2];
            axis = axis0;
            for (int k = 0; k < 2; k++)
                x[k] = x0[k];

            Left = Right = Parent = null;
            visited = false;
            id = 0;
        }

        public Node FindParent(double[] x0)
        {
            Node parent = null;
            Node next = this;
            int split;
            while (next != null)
            {
                split = next.axis;
                parent = next;
                if (x0[split] > next.x[split])
                    next = next.Right;
                else
                    next = next.Left;
            }
            return parent;
        }

        public Node Insert(double[] p)
        {
            Node parent = FindParent(p);
            if (Equal(p, parent.x, 2) == true)
                return null;

            Node newNode = new Node(p, parent.axis + 1 < 2 ? parent.axis + 1
                    : 0);
            newNode.Parent = parent;

            if (p[parent.axis] > parent.x[parent.axis])
            {
                parent.Right = newNode;
                newNode.orientation = true; //
            }
            else
            {
                parent.Left = newNode;
                newNode.orientation = false; //
            }

            return newNode;
        }

        internal bool Equal(double[] x1, double[] x2, int dim)
        {
            for (int k = 0; k < dim; k++)
            {
                if (x1[k] != x2[k])
                    return false;
            }

            return true;
        }

        internal double Distance2(double[] x1, double[] x2, int dim)
        {
            //double S = 0;
            //for (int k = 0; k < dim; k++)
            //    S += (x1[k] - x2[k]) * (x1[k] - x2[k]);
            //return S;

            double s2 = 0;
            s2 = Utilties.GeoCalculator.CalculateDistanceInMeters(
                 new Models.Location { Latitude = x1[0], Longitude = x1[1] },
                 new Models.Location { Latitude = x2[0], Longitude = x2[1] });

            return Convert.ToSingle(s2);
        }
    }

    class KDTree
    {
        Node Root;
        double d_min;
        Node nearest_neighbour;

        int KD_id;

        int nList;

        Node[] visitedNodes;
        int visited_nodes;
        Node[] List;

        double[] x_min, x_max;
        bool[] max_boundary, min_boundary;
        int n_boundary;

        public KDTree(int i)
        {
            Root = null;
            KD_id = 1;
            nList = 0;
            List = new Node[i];
            visitedNodes = new Node[i];
            max_boundary = new bool[2];
            min_boundary = new bool[2];
            x_min = new double[2];
            x_max = new double[2];
        }

        public bool Add(double[] x)
        {
            x[0] = Math.Round(x[0], 5);
            x[1] = Math.Round(x[1], 5);

            if (nList >= 2000000 - 1)
                return false;

            if (Root == null)
            {
                Root = new Node(x, 0);
                Root.id = KD_id++;
                List[nList++] = Root;
            }
            else
            {
                Node pNode;
                if ((pNode = Root.Insert(x)) != null)
                {
                    pNode.id = KD_id++;
                    List[nList++] = pNode;
                }
            }

            return true;
        }

        public Node find_nearest(double[] x)
        {
            if (Root == null)
                return null;

            visited_nodes = 0;
            Node parent = Root.FindParent(x);
            nearest_neighbour = parent;
            d_min = Root.Distance2(x, parent.x, 2);


            if (parent.Equal(x, parent.x, 2) == true)
                return nearest_neighbour;

            search_parent(parent, x);
            uncheck();

            return nearest_neighbour;
        }

        public void check_subtree(Node node, double[] x)
        {
            if ((node == null) || node.visited)
                return;

            visitedNodes[visited_nodes++] = node;
            node.visited = true;
            set_bounding_cube(node, x);

            int dim = node.axis;
            double d = node.x[dim] - x[dim];

            if (d * d > d_min)
            {
                if (node.x[dim] > x[dim])
                    check_subtree(node.Left, x);
                else
                    check_subtree(node.Right, x);
            }
            else
            {
                check_subtree(node.Left, x);
                check_subtree(node.Right, x);
            }
        }

        public void set_bounding_cube(Node node, double[] x)
        {
            if (node == null)
                return;
            int d = 0;
            double dx;
            for (int k = 0; k < 2; k++)
            {
                dx = node.x[k] - x[k];
                if (dx > 0)
                {
                    dx *= dx;
                    if (!max_boundary[k])
                    {
                        if (dx > x_max[k])
                            x_max[k] = dx;
                        if (x_max[k] > d_min)
                        {
                            max_boundary[k] = true;
                            n_boundary++;
                        }
                    }
                }
                else
                {
                    dx *= dx;
                    if (!min_boundary[k])
                    {
                        if (dx > x_min[k])
                            x_min[k] = dx;
                        if (x_min[k] > d_min)
                        {
                            min_boundary[k] = true;
                            n_boundary++;
                        }
                    }
                }
                d += Convert.ToInt32(dx);
                if (d > d_min)
                    return;

            }

            if (d < d_min)
            {
                d_min = d;
                nearest_neighbour = node;
            }
        }

        public Node search_parent(Node parent, double[] x)
        {
            for (int k = 0; k < 2; k++)
            {
                x_min[k] = x_max[k] = 0;
                max_boundary[k] = min_boundary[k] = false; //
            }
            n_boundary = 0;

            Node search_root = parent;
            while (parent != null && (n_boundary != 2 * 2))
            {
                check_subtree(parent, x);
                search_root = parent;
                parent = parent.Parent;
            }

            return search_root;
        }

        public void uncheck()
        {
            for (int n = 0; n < visited_nodes; n++)
                visitedNodes[n].visited = false;
        }

    }
}
