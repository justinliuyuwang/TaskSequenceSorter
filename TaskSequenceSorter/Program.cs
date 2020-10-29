/*****
 * FileName: Program.cs
 * FileType: Visual C# Source file
 * Author: Justin Wang
 * Created On: 23/9/2020
 * Last modified on: 24/9/2020
 * Description: The Main function in this class outputs a JSON file containing a sorted sequence of tasks based on dependencies detailed in a user-provided JSON file.
 * This program uses a modified version of Kahn's algorithm on a directed graph to find the appropriate order of tasks.
 *****/

using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;


namespace TaskSequenceSorter
{
    class Program
    {
        static void Main(string[] args)
        {

            /**********************
            * PROMPTING USER INPUT
            ***********************/
            
            Console.WriteLine("Input the full path to your JSON file: ");
            string filepath = Console.ReadLine();



            /*************************
            * READING FROM INPUT FILE
            **************************/

            string JSON_input = "";   

            try { 
                JSON_input = File.ReadAllText(filepath);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found!");
            }
            catch (Exception e)
            {
                Console.WriteLine("INPUT ERROR: ");
                Console.WriteLine(e.Message);
            }



            /**************************
            * PARSING TASK DEPENDENCIES
            ***************************/

            //Initializing object to hold dependency information deserialized from JSON input
            DependencyJSON jDependency = new DependencyJSON();

            try
            {
                jDependency = JsonConvert.DeserializeObject<DependencyJSON>(JSON_input);   //Deserialization
            }
            catch (Exception e)
            {
                Console.WriteLine("PARSING ERROR: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("JSON_input is: " + JSON_input);  //Error could potentially be caused by a bad JSON input
            }
            


            /*************************
            * CREATING DIRECTED GRAPH
            **************************/
            // An adjacency list representation was chosen over a matrix representation for the directed graph due to differences in storage space and performance at larger input sizes (in the context of TaskSequenceSorter and its implementation of a modified Kahn's algorithm).


            //Dictionary to assign an integer to each task for use in graph adjacency list
            Dictionary<string, int> TaskDictionary = new Dictionary<string, int>();

            //List to assign each task to its associated integer via index to avoid looking up key by value in TaskDictionary
            List<string> TaskList = new List<string>();


            int NumberOfTasks = 0;

            //Count the number of unique tasks and assign each to an integer (using both the dictionary and list)
            try
            {
                foreach (var item in jDependency.TaskParentChildPairs)
                {
                    if (!TaskDictionary.ContainsKey(item.ChildTask))    //If this child task name isn't in the dictionary then 
                    {
                        TaskDictionary.Add(item.ChildTask, NumberOfTasks);  //Add to dictionary (task # is value, task name is key)
                        TaskList.Add(item.ChildTask);                       //Add to list (task # is index)
                        NumberOfTasks++;


                    }

                    if (!TaskDictionary.ContainsKey(item.ParentTask))   //Repeat on parent tasks
                    {
                        TaskDictionary.Add(item.ParentTask, NumberOfTasks);
                        TaskList.Add(item.ParentTask);
                        NumberOfTasks++;

                    }

                }
            }
            catch (Exception e) //potential JSON object error (maybe model doesn't match JSON data)
            {
                Console.WriteLine(e.Message);
            }


            //Initialize graph with a vertex for each task
            Graph TaskGraph = new Graph(NumberOfTasks);


            //Add edges to the graph (using task #)
            try { 
                foreach (var item in jDependency.TaskParentChildPairs)
                {
                    try { 
                        TaskGraph.AddEdge(TaskDictionary[item.ChildTask], TaskDictionary[item.ParentTask]);     //Add edge with child task pointing to its parent task. Record this edge on the child vertex. If a vertex has edges then the task it represents depends on other vertices/tasks.
                    }
                    catch (Exception e) //Error in case we access a non-existent key or come across an error adding edges to the graph
                    {
                        Console.WriteLine("GRAPH FILLING/DICTIONARY ACCESS ERROR: ");
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception e) //potential JSON object error (maybe model doesn't match JSON data)
            {
                Console.WriteLine(e.Message);
            }




            /**************************
            * MODIFIED KAHN'S ALGORITHM
            ***************************/


            //Instantiate output holder to contain the sequence of tasks 
            TaskSequence tasksequenceobject = new TaskSequence();


            //Flags to signal whether:
            bool Finished = false;      //We have no more valid vertices left (no more vertices without remaining prerequisites)
            bool Empty;         //We have no more vertices left
           
            //List to hold valid vertices (vertices we can add to our tasksequence/output holder because they have no more prerequisites)
            List<int> Valid = new List<int>();

            //Find all valid vertices at initial state by iterating through all vertices in adjacency list and adding the ones with no edges to the Valid list 
            int i = 0;
            while (i < NumberOfTasks)
            {
                if (TaskGraph.AdjacencyList[i].Count == 0)
                {
                    Valid.Add(i);
                }
                i++;
            }


            //Loop while we still have valid vertices to visit
            while (!Finished) {
                
                //Default to finished if we don't encounter any valid vertices
                Finished = true;


                //REMOVING VALID TASKS AND ADDING THEM TO SEQUENCE OBJECT
                List<string> CurrentNodes = new List<string>(); //List to hold vertices to be removed this iteration

                for (int q = Valid.Count-1; q>-1; q--) //Iterate through valid vertices (backwards because we are going to be removing items from this list)
                {
                    
                        Finished = false;                                                       //We are not finished yet, still need to iterate again to look for new vertices that are newly valid once this vertex's dependency edges are removed
                        string myKey = TaskList[Valid[q]];       //Find the task name associated with the vertex number
                        CurrentNodes.Add(myKey);                  //Add this task name to a temp list to be added to the sequence object later
                                                                                  
                        Valid.Remove(Valid[q]);    //Remove this vertex from the list of valid vertices
                    

                }


                //RELEASING REMOVED TASKS' ASSOCIATED DEPENDENCY EDGES AND FILLING VALID LIST
                //remove vertices and remove edges that point to these vertices
                foreach (var item in CurrentNodes)                                      //For each vertex removed/visited during this iteration of the while loop
                {
                    int k = 0;  

                   
                    while (k < NumberOfTasks) {     //Iterate through all vertices. k represents vertex number as well as adjacency list index.
                        
                        List<int> NewVertexEdges = TaskGraph.AdjacencyList[k];   
                        
                        if (NewVertexEdges.Contains(TaskDictionary[item]))  //If a vertex points to our newly removed vertex
                        {
                            NewVertexEdges.Remove(TaskDictionary[item]);    //Remove the edge pointing to the newly removed vertex
                            if (NewVertexEdges.Count == 0)                  //If this vertex no longer has edges then it is valid for removal next iteration
                            {
                                Valid.Add(k);
                               
                            }
                        }
                        
                        k++;
                    }
                }
                

                //ADDING TASKS TO OUTPUT HOLDER
                //Add the task names of the vertices we visited and removed this iteration, if any.
                //If we still haven't visited any valid vertices this loop, then there is nothing to add to the task sequence array/output holder.
                if (!Finished) { 
                    tasksequenceobject.TaskSequenceArray.Add(CurrentNodes);
                }


                //CHECKING FOR EMPTY GRAPH
                int m = 0;
                Empty = true;   //Default empty
                while (m < NumberOfTasks)   //Iterate through all the vertices
                {
                    
                    if (!(TaskGraph.AdjacencyList[m].Count==0))     //If any of the vertices does not contain an empty list of edges then our graph is not empty
                    {
                        Empty = false;
                    }

                    m++;
                }


                //CHECKING FOR CYCLES
                //If we don't have any more valid vertices to visit but the graph is not empty yet, then there is a cycle in our graph and we cannot resolve the dependencies.
                if ((Finished)&&(!Empty)){
                    Console.WriteLine("\nERROR:");
                    Console.WriteLine("Cyclical dependencies present. The task sequence cannot be sorted\n");
                    return;
                }
               

            }



            /******************
            * SERIALIZE OUTPUT
            *******************/

            //Convert the task sequence object to a JSON output
            string JSON_output = JsonConvert.SerializeObject(tasksequenceobject, Formatting.Indented);
            



            /****************
             * WRITE TO FILE
             ****************/

            //Place output file in the same folder as input. Use the same filename, except with "_output" at the end.
            filepath = filepath.Remove(filepath.Length - 5);
            filepath += "_output.JSON";

            try
            {
                File.WriteAllText(filepath, JSON_output);
                Console.WriteLine();
                Console.WriteLine("Your output file is: " + filepath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            



            return;
        }


    }
}
