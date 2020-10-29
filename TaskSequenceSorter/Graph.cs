/*****
 * FileName: Graph.cs
 * FileType: Visual C# Source file
 * Author: Justin Wang
 * Created On: 23/9/2020
 * Last modified on: 24/9/2020
 * Description: This class is an implementation of an adjacency list of a directed graph. 
 *****/

using System;
using System.Collections.Generic;

namespace TaskSequenceSorter
{
    class Graph 
    {
      
        public List<List<int>> AdjacencyList { get; set; }


        //Constructor 
        public Graph(int NumberOfVertices) 
        {
            AdjacencyList = new List<List<int>>();

            //Fill the adjacency list with an empty list for each vertex
            for (int j = 0; j < NumberOfVertices; j++)  
            {
                List<int> EdgeList = new List<int>();
                AdjacencyList.Add(EdgeList);
            }
        }


        //Adds an edge to the adjacency list
        public void AddEdge(int Tail, int Head) 
        {
            
            AdjacencyList[Tail].Add(Head);  //Tail is where the edge originates from and Head is where the edge points
        }



        //Prints a visual representation of the graph to console by iterating through the adjacency list. 
        public void PrintGraph() { 

            for (int i = 0; i < AdjacencyList.Count; i++)       //i is vertex
            {
                Console.WriteLine("Adjacency list for vertex " + i);
                
                for (int j = 0; j < AdjacencyList[i].Count; j++)        
                {
                    if (j != 0) { 
                        Console.WriteLine(" -> " + AdjacencyList[i][j]);    //AdjacencyList[i][j] is the vertex pointed to by vertex i
                    }
                    else { 
                        Console.WriteLine(" -> " + AdjacencyList[i][j]);
                    }
                }
                Console.WriteLine();
            }

        }

        

    }
}
