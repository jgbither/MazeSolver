/*
*   Josh Bither
*/


using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(String[] args)
        {
            try
            {
                Bitmap bp = new Bitmap(args[0]);
                String mazeResult = args[1];

                int width = bp.Width;
                int height = bp.Height;

                //An array of nodes are created to represent every pixel of the Bitmap
                //This is done in order to track the path back to the entrance of the maze
                Node[,] nodes = new Node[width, height];

                Console.WriteLine("Height: " + height + " Width: " + width);
                int startPointX = -1;
                int startPointY = 0;


                //This loop goes through and initiliazes each node with a pixel and finds the entrance to the maze
                Console.WriteLine("Preparing maze...");
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {

                        nodes[x, y] = new Node(bp.GetPixel(x, y), x, y);

                        //Node.isRed is a static method that simply checks for color of the node
                        if (Node.isRed(bp.GetPixel(x, y)))
                        {
                            if (startPointX == -1)
                            {
                                startPointX = x;
                                startPointY = y;
                            }
                        }

                    }
                }



                //The maze solution is found using Breadth First Search, hench the Queue
                int count = 0;
                Color green = Color.ForestGreen;
                Queue<Node> q = new Queue<Node>();

                bool isSolved = false;

                Console.WriteLine("Starting location, x:" + startPointX + " y:" + startPointY);
                Console.WriteLine("Solving maze...");

                //The queue is initialized with the starting location
                nodes[startPointX, startPointY].setChecked(true);
                q.Enqueue(nodes[startPointX, startPointY]);

                while (q.Count > 0)
                {
                    Node current = q.Dequeue();
                    count++;

                    //Solution to maze found
                    if (Node.isBlue(current.getColor()))
                    {
                        isSolved = true;
                        Console.WriteLine("Exit of maze found! Now drawing solution...");

                        //The solution is drawn by following the chain of parent nodes back to the beginning
                        //It colors the line green, as well as making the adjacent nodes green to add thickness to the line
                        while (current.getParentNode() != null)
                        {

                            //This is the original 1-pixel path that is set green
                            bp.SetPixel(current.getLocationX(), current.getLocationY(), green);


                            //These 4 blocks make the green line thicker, checking that the adjacent nodes aren't black first
                            if (!Node.isBlack(bp.GetPixel(current.getLocationX() + 1, current.getLocationY())))
                            {
                                bp.SetPixel(current.getLocationX() + 1, current.getLocationY(), green);
                                if (!Node.isBlack(bp.GetPixel(current.getLocationX() + 2, current.getLocationY())))
                                {
                                    bp.SetPixel(current.getLocationX() + 2, current.getLocationY(), green);
                                }
                            }

                            if (!Node.isBlack(bp.GetPixel(current.getLocationX(), current.getLocationY() + 1)))
                            {
                                bp.SetPixel(current.getLocationX(), current.getLocationY() + 1, green);
                                if (!Node.isBlack(bp.GetPixel(current.getLocationX(), current.getLocationY() + 2)))
                                {
                                    bp.SetPixel(current.getLocationX(), current.getLocationY() + 2, green);
                                }
                            }

                            if (!Node.isBlack(bp.GetPixel(current.getLocationX() - 1, current.getLocationY())))
                            {
                                bp.SetPixel(current.getLocationX() - 1, current.getLocationY(), green);
                                if (!Node.isBlack(bp.GetPixel(current.getLocationX() - 2, current.getLocationY())))
                                {
                                    bp.SetPixel(current.getLocationX() - 2, current.getLocationY(), green);
                                }
                            }

                            if (!Node.isBlack(bp.GetPixel(current.getLocationX(), current.getLocationY() - 1)))
                            {
                                bp.SetPixel(current.getLocationX(), current.getLocationY() - 1, green);
                                if (!Node.isBlack(bp.GetPixel(current.getLocationX(), current.getLocationY() - 2)))
                                {
                                    bp.SetPixel(current.getLocationX(), current.getLocationY() - 2, green);
                                }
                            }

                            //Console.WriteLine("x: " + current.getLocationX() + " y: " + current.getLocationY()); 
                            current = current.getParentNode();
                        }
                        q.Clear();
                        break;

                    }






                    //These 4 blocks check the 4 immediate nodes/pixels surrounding the current node
                    //It confirms that the node hasn't been checked yet, and then makes sure the node is not black before enqueing
                    if (!q.Contains(nodes[current.getLocationX() + 1, current.getLocationY()]))
                    {
                        if (!nodes[current.getLocationX() + 1, current.getLocationY()].hasBeenChecked())
                        {
                            nodes[current.getLocationX() + 1, current.getLocationY()].setChecked(true);
                            if (!Node.isBlack(nodes[current.getLocationX() + 1, current.getLocationY()].getColor()))
                            {
                                //The "Parent Node" is set in order to trace the path back from the solution
                                nodes[current.getLocationX() + 1, current.getLocationY()].setParentNode(current);
                                q.Enqueue(nodes[current.getLocationX() + 1, current.getLocationY()]);
                            }
                        }
                    }

                    if (!q.Contains(nodes[current.getLocationX(), current.getLocationY() + 1]))
                    {
                        if (!nodes[current.getLocationX(), current.getLocationY() + 1].hasBeenChecked())
                        {
                            nodes[current.getLocationX(), current.getLocationY() + 1].setChecked(true);
                            if (!Node.isBlack(nodes[current.getLocationX(), current.getLocationY() + 1].getColor()))
                            {

                                nodes[current.getLocationX(), current.getLocationY() + 1].setParentNode(current);
                                q.Enqueue(nodes[current.getLocationX(), current.getLocationY() + 1]);
                            }
                        }
                    }

                    if (!q.Contains(nodes[current.getLocationX() - 1, current.getLocationY()]))
                    {
                        if (!nodes[current.getLocationX() - 1, current.getLocationY()].hasBeenChecked())
                        {
                            nodes[current.getLocationX() - 1, current.getLocationY()].setChecked(true);
                            if (!Node.isBlack(nodes[current.getLocationX() - 1, current.getLocationY()].getColor()))
                            {
                                nodes[current.getLocationX() - 1, current.getLocationY()].setParentNode(current);
                                q.Enqueue(nodes[current.getLocationX() - 1, current.getLocationY()]);
                            }
                        }
                    }

                    if (!q.Contains(nodes[current.getLocationX(), current.getLocationY() - 1]))
                    {
                        if (!nodes[current.getLocationX(), current.getLocationY() - 1].hasBeenChecked())
                        {
                            nodes[current.getLocationX(), current.getLocationY() - 1].setChecked(true);
                            if (!Node.isBlack(nodes[current.getLocationX(), current.getLocationY() - 1].getColor()))
                            {
                                nodes[current.getLocationX(), current.getLocationY() - 1].setParentNode(current);
                                q.Enqueue(nodes[current.getLocationX(), current.getLocationY() - 1]);
                            }
                        }
                    }

                }

                //Makes sure that the blue solution was found
                if (isSolved)
                {
                    bp.Save(mazeResult);
                    Console.WriteLine("DONE! Saved as " + mazeResult);
                }
                else
                {
                    Console.WriteLine("Solution to maze not found!");
                }


            }
            catch (ArgumentException e)
            {
                Console.WriteLine("File error, file not loaded correctly. Try again");
            }

            Console.ReadLine();

        }
       
    }



    //Each Node represents a single pixel on the bitmap
    //This was created in order to keep track of the "Parent Node", allowing each node to be traced to the entrance
    //Also tracks whether the node has already been checked into the queue
    class Node
    {
        private Color color;
        private Node parentNode;
        private int locationX;
        private int locationY;
        private bool beenChecked;


        public Node(Color color, int x, int y)
        {
            parentNode = null;
            this.color = color;
            locationX = x;
            locationY = y;
            beenChecked = false;
        }

        public Color getColor()
        {
            return color;
        }

        public Node getParentNode()
        {
            return parentNode;
        }

        public int getLocationX()
        {
            return locationX;
        }

        public int getLocationY()
        {
            return locationY;
        }

        public void setParentNode(Node node)
        {
            parentNode = node;
        }

        public void setColor(Color color)
        {
            this.color = color;
           
        }

        public void setChecked(bool check)
        {
            beenChecked = check;
        }

        public bool hasBeenChecked()
        {
            return beenChecked;
        }


        //These 3 static methods simply check for their color
        //Originally they only checked for one solid color, but now check for a range of values
        //Were made as static methods because of clear, simplistic use
        public static bool isBlack(Color color)
        {
            int deltaR = 0 - color.R;
            int deltaG = 0 - color.G;
            int deltaB = 0 - color.B;

            if (Math.Abs(deltaR) + Math.Abs(deltaG) + Math.Abs(deltaB) <= 150)
                return true;
            else
                return false;
        }

        public static bool isBlue(Color color)
        {
            int deltaR = 0 - color.R;
            int deltaG = 0 - color.G;
            int deltaB = 255 - color.B;

            if (Math.Abs(deltaR) + Math.Abs(deltaG) + Math.Abs(deltaB) <= 180)
                return true;
            else
                return false;
        }

        public static bool isRed(Color color)
        {
            int deltaR = 255 - color.R;
            int deltaG = 0 - color.G;
            int deltaB = 0 - color.B;

            if (Math.Abs(deltaR) + Math.Abs(deltaG) + Math.Abs(deltaB) <= 180)
                return true;
            else
                return false;
        }
    }
}
