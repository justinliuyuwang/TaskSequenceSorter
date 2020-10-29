/*****
 * FileName: DependencyJSON.cs
 * FileType: Visual C# Source file
* Author: Justin Wang
* Created On: 23/9/2020
* Last modified on: 24/9/2020
* Description: This class is a model to deserialize an input JSON file onto.
*****/

using System.Collections.Generic;


namespace TaskSequenceSorter
{
    public class DependencyJSON
    {
        public List<Dependency> TaskParentChildPairs { get; set; }    //Each ParentTask-ChildTask pair describes a dependency
    }

    public class Dependency
    {
        public string ParentTask { get; set; }      //ChildTask depends on ParentTask
        public string ChildTask { get; set; }
    }
}
