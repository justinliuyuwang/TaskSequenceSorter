/*****
 * FileName: TaskSequence.cs
 * FileType: Visual C# Source file
 * Author: Justin Wang
 * Created On: 23/9/2020
 * Last modified on: 24/9/2020
 * Description: This class is a holder for a sorted sequence of tasks to facilitate simple serialization and output as a JSON.
 *****/

using System.Collections.Generic;


namespace TaskSequenceSorter
{
    class TaskSequence
    {

        //The sequence of tasks is a 2d-list because some tasks may be completed simultaneously (e.g. five tasks may be available at the start, so an array of five tasks is the first item in the task sequence array)
        public List<List<string>> TaskSequenceArray { get; set; } = new List<List<string>>();   

    }
}
