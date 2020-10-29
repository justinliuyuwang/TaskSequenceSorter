# TaskSequenceSorter

TaskSequenceSorter outputs a JSON file containing a sorted sequence of tasks based on dependencies detailed in a user-provided JSON file.  The program uses a modified version of Kahn's algorithm on a directed graph to find the appropriate order of tasks.


# Getting Started


## Run TaskSequenceSorter.exe
From the main *TaskSequenceSorter* directory, go to _TaskSequenceSorter\bin\Release\netcoreapp3.1\publish_ and run **TaskSequenceSorter.exe**.

## Input File Path
The console will prompt you for a full file path to the JSON input file containing your task dependencies.

## Output
Your output file will be in the same folder as your input file.


## Sample Input File

>{
	 "TaskParentChildPairs": [
		    {
			      "ParentTask": "Vision Statement",
			      "ChildTask": "Mission Statement"
		    },
		    {
			      "ParentTask": "Mission Statement",
			      "ChildTask": "Funding"
		    },
		    {
			      "ParentTask": "Requirements Gathering",
			      "ChildTask": "Analysis"
		    },
		    {
			      "ParentTask": "Requirements Gathering",
			      "ChildTask": "Initial Estimate"
		    }
	  ]
}


## Sample Output File

>{
	  "TaskSequenceArray": [
		    [
			      "Vision Statement",
			      "Requirements Gathering"
		    ],
		    [
			      "Mission Statement",
			      "Analysis",
			      "Initial Estimate"
		    ],
		    [
			      "Funding"
		    ]
	  ]
}


# Important Files

**Published console application** can be found in:
 >*TaskSequenceSorter\TaskSequenceSorter\bin\Release\netcoreapp3.1\publish*


**Source files (.cs)** can be found in:
>*TaskSequenceSorter\TaskSequenceSorter*

**Sample inputs/outputs** can be found in:
>*TaskSequenceSorter\sample_input_output*
